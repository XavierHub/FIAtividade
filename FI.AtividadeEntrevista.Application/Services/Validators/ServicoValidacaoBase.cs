using FI.AtividadeEntrevista.Aplicacao.Services.Validators;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FI.AtividadeEntrevista.Dominio.Enumeradores;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading.Tasks;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public abstract class ServicoValidacaoBase<T>
    {
        protected readonly InlineValidator<T> _validador;
        private readonly IServicoNotificacao _servicoNotificacao;

        protected ServicoValidacaoBase(IServicoNotificacao notifierService)
        {
            _validador = new InlineValidator<T>();
            _servicoNotificacao = notifierService;
        }

        public virtual async Task<bool> Validar(TipoValidacao tipoValidacao, T model)
        {
            if (model == null)
            {
                _servicoNotificacao.Adicionar(new ValidationResult(new[] { new ValidationFailure("", "Model is null") }));
                return false;
            }

            var validateResultModel = await _validador.ValidateAsync(model);
            _servicoNotificacao.Adicionar(validateResultModel);

            return validateResultModel.IsValid;
        }

        public async Task<bool> ValorValido<TValue>(string name, TValue value, Func<TValue, bool> validationRule, string errorMessage = "The '{PropertyName}' has an invalid value")
        {
            var validator = new InlineValidator<TValue>();
            if (value == null)
            {
                _servicoNotificacao.Adicionar(new ValidationResult(new[] { new ValidationFailure(name, "The '{PropertyName}' property cannot be null") }));
                return false;
            }

            validator.RuleFor(x => x)
                .Must(validationRule)
                .WithName(name)
                .WithMessage(errorMessage);

            var validateResultModel = await validator.ValidateAsync(value);
            _servicoNotificacao.Adicionar(validateResultModel);

            return validateResultModel.IsValid;
        }

        public virtual async Task<bool> CPFValido(string value, string errorMessage = "O CPF está inválido")
        {
            var validator = new ValidadorCPF(errorMessage);

            if (value == null)
            {
                _servicoNotificacao.Adicionar(new ValidationResult(new[] { new ValidationFailure("CPF", "O CPF é obrigatório") }));
                return false;
            }

            var validateResult = await validator.ValidateAsync(value);
            _servicoNotificacao.Adicionar(validateResult);
            return validateResult.IsValid;
        }
    }
}
