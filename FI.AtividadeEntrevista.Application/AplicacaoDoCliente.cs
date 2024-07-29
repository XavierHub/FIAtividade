using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao.Repositorios;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FI.AtividadeEntrevista.Dominio.Enumeradores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Application
{
    public class AplicacaoDoCliente : IAplicacaoDoCliente
    {
        private readonly IRepositorio<Cliente> _clienteRepositorio;
        private readonly IServicoValidacao<Cliente> _servicoValidacao;
        private readonly IServicoNotificacao _servicoNotificacao;

        public AplicacaoDoCliente(IRepositorio<Cliente> clienteRepositorio, IServicoValidacao<Cliente> servicoValidacao, IServicoNotificacao servicoNotificacao)
        {
            _clienteRepositorio = clienteRepositorio;
            _servicoValidacao = servicoValidacao;
            _servicoNotificacao = servicoNotificacao;
        }

        public async Task<Cliente> ObterPorId(long id)
        {
            if (!await _servicoValidacao.ValorValido(nameof(id), id, (x) => x is long intValue && intValue > 0))
                return new Cliente();

            return await _clienteRepositorio.ExecutarProcedure<Cliente>("FI_SP_ConsCliente", new { id });
        }

        public async Task<Cliente> Inserir(Cliente model)
        {
            if (!await _servicoValidacao.Validar(TipoValidacao.Inserir, model))
                return new Cliente();

            var cpfExiste = await _clienteRepositorio.ExecutarProcedure<bool>("FI_SP_VerificaCliente", new { model.CPF });
            if (cpfExiste)
            {
                _servicoNotificacao.Adicionar("CPF", $"O CPF {model.CPF} já está cadastrado.");
                return new Cliente();
            }

            model.Id = await _clienteRepositorio.ExecutarProcedure<long>("FI_SP_IncClienteV2",
                new
                {
                    model.Nome,
                    model.CPF,
                    model.Sobrenome,
                    model.Nacionalidade,
                    model.CEP,
                    model.Estado,
                    model.Cidade,
                    model.Logradouro,
                    model.Email,
                    model.Telefone
                });
            return model;
        }

        public async Task<bool> Alterar(long id, Cliente model)
        {
            if (!await _servicoValidacao.Validar(TipoValidacao.Alterar, model) ||
                !await _servicoValidacao.ValorValido(nameof(id), id, (x) => x is long intValue && intValue > 0)
               )
                return false;

            var entity = await _clienteRepositorio.ObterPorId(id);
            if (entity.Id == 0)
                return false;

            model.Id = entity.Id;

            await _clienteRepositorio.ExecutarProcedure<long>("FI_SP_AltCliente",
                        new
                        {
                            model.Nome,
                            model.CPF,
                            model.Sobrenome,
                            model.Nacionalidade,
                            model.CEP,
                            model.Estado,
                            model.Cidade,
                            model.Logradouro,
                            model.Email,
                            model.Telefone,
                            model.Id
                        });

            return true;
        }

        public async Task<(IEnumerable<Cliente>, int)> Pesquisar(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente)
        {
            return await _clienteRepositorio.ConsultarMultSelecao<int>("FI_SP_PesqCliente", new { iniciarEm, quantidade, campoOrdenacao, crescente });
        }
    }
}
