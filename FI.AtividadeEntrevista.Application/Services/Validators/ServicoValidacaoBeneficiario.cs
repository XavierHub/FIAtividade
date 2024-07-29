using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FI.AtividadeEntrevista.Dominio.Enumeradores;
using FluentValidation;
using System.Threading.Tasks;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public class ServicoValidacaoBeneficiario : ServicoValidacaoBase<Beneficiario>, IServicoValidacao<Beneficiario>
    {
        public ServicoValidacaoBeneficiario(IServicoNotificacao servicoNotificacao) : base(servicoNotificacao)
        {
            _validador.RuleFor(x => x.Nome).NotNull().NotEmpty().MaximumLength(80);
        }

        public override async Task<bool> Validar(TipoValidacao tipoValidacao, Beneficiario model)
        {
            return await base.Validar(tipoValidacao, model);
        }
    }
}
