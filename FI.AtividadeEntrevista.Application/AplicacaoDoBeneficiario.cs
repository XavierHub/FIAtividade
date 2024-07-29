using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao.Repositorios;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FI.AtividadeEntrevista.Dominio.Enumeradores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Application
{
    public class AplicacaoDoBeneficiario : IAplicacaoDoBeneficiario
    {
        private readonly IRepositorio<Beneficiario> _beneficiarioRepositorio;
        private readonly IServicoValidacao<Beneficiario> _servicoValidacao;
        private readonly IServicoNotificacao _servicoNotificacao;

        public AplicacaoDoBeneficiario(IRepositorio<Beneficiario> beneficiarioRepositorio, IServicoValidacao<Beneficiario> servicoValidacao, IServicoNotificacao servicoNotificacao)
        {
            _beneficiarioRepositorio = beneficiarioRepositorio;
            _servicoValidacao = servicoValidacao;
            _servicoNotificacao = servicoNotificacao;
        }

        public async Task<IEnumerable<Beneficiario>> ConsultarPorIdCliente(long idCliente)
        {
            if (!await _servicoValidacao.ValorValido(nameof(idCliente), idCliente, (x) => x is long intValue && intValue > 0))
                return new List<Beneficiario>();

            return await _beneficiarioRepositorio.Consultar(x => x.IdCliente == idCliente);
        }

        public async Task<bool> Alterar(long idCliente, List<Beneficiario> models)
        {
            // Validações iniciais
            if (!await _servicoValidacao.ValorValido(nameof(idCliente), idCliente, (x) => x is long intValue && intValue > 0))
                return false;

            foreach (var model in models)
            {
                if (!await _servicoValidacao.Validar(TipoValidacao.Alterar, model) ||
                    !await _servicoValidacao.CPFValido(model.CPF))
                {
                    return false;
                }
            }

            // Consulta os beneficiários existentes no banco de dados para o cliente
            var entidades = await _beneficiarioRepositorio.Consultar(x => x.IdCliente == idCliente);

            // Exclui os beneficiários que estão no banco de dados, mas não estão na lista de modelos fornecida
            foreach (var entidade in entidades)
            {
                if (!models.Any(m => m.Id == entidade.Id))
                {
                    await _beneficiarioRepositorio.ExecutarProcedure<long>("FI_SP_DelBeneficiario", new { entidade.Id });
                }
            }

            // Atualiza ou insere os beneficiários fornecidos na lista de modelos
            foreach (var item in models)
            {
                if (item.Id > 0)
                {
                    await _beneficiarioRepositorio.ExecutarProcedure<long>("FI_SP_AltBeneficiario", new { item.Id, item.CPF, item.Nome, idCliente });
                }
                else
                {
                    if (entidades.Any(x => x.CPF == item.CPF))
                    {
                        _servicoNotificacao.Adicionar("CPF", $"O CPF {item.CPF} do Beneficiário já está cadastrado");

                    }
                    await _beneficiarioRepositorio.ExecutarProcedure<long>("FI_SP_IncBeneficiario", new { item.CPF, item.Nome, idCliente });
                }
            }
            return true;
        }

    }
}
