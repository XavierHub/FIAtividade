using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FI.AtividadeEntrevista.Dominio.Enumeradores;
using FluentValidation;
using System.Threading.Tasks;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public class ServicoValidacaoCliente : ServicoValidacaoBase<Cliente>, IServicoValidacao<Cliente>
    {
        public ServicoValidacaoCliente(IServicoNotificacao servicoNotificacao) : base(servicoNotificacao)
        {
            _validador.RuleFor(x => x.Nome).NotNull().NotEmpty().MaximumLength(80);
            _validador.RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(50).EmailAddress();
        }

        public override async Task<bool> Validar(TipoValidacao tipoValidacao, Cliente model)
        {
            return await base.Validar(tipoValidacao, model) && await CPFValido(model.CPF);
        }
    }
}
