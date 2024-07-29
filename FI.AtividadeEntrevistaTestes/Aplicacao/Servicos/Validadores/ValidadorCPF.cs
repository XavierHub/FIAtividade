using FI.AtividadeEntrevista.Aplicacao.Services.Validators;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FI.AtividadeEntrevista.Tests.Services.Validators
{
    [TestClass]
    public class ValidadorCPFTests
    {
        private ValidadorCPF _validadorCPF;

        [TestInitialize]
        public void Initialize()
        {
            _validadorCPF = new ValidadorCPF("O CPF está inválido");
        }

        [TestMethod]
        public void CPF_Valido_DevePassar()
        {
            // Arrange
            var cpfValido = "123.456.789-09"; // Substitua por um CPF válido

            // Act & Assert
            var result = _validadorCPF.TestValidate(cpfValido);
            result.ShouldNotHaveValidationErrorFor(cpf => cpf);
        }

        [TestMethod]
        public void CPF_Vazio_DeveRetornarErro()
        {
            // Act & Assert
            var result = _validadorCPF.TestValidate("");
            result.ShouldHaveValidationErrorFor(cpf => cpf).WithErrorMessage("O CPF é obrigatório");
        }

        [TestMethod]
        public void CPF_Repetido_DeveRetornarErro()
        {
            // Arrange
            var cpfRepetido = "111.111.111-11";

            // Act & Assert
            var result = _validadorCPF.TestValidate(cpfRepetido);
            result.ShouldHaveValidationErrorFor(cpf => cpf).WithErrorMessage("O CPF está inválido");
        }

        [TestMethod]
        public void CPF_Invalido_DeveRetornarErro()
        {
            // Arrange
            var cpfInvalido = "123.456.789-00"; // Substitua por um CPF inválido

            // Act & Assert
            var result = _validadorCPF.TestValidate(cpfInvalido);
            result.ShouldHaveValidationErrorFor(cpf => cpf).WithErrorMessage("O CPF está inválido");
        }

        [TestMethod]
        public void CPF_ComCaracteresInvalidos_DeveRetornarErro()
        {
            // Arrange
            var cpfInvalido = "123.abc.789-09";

            // Act & Assert
            var result = _validadorCPF.TestValidate(cpfInvalido);
            result.ShouldHaveValidationErrorFor(cpf => cpf).WithErrorMessage("O CPF está inválido");
        }

        [TestMethod]
        public void CPF_MenosDigitos_DeveRetornarErro()
        {
            // Arrange
            var cpfInvalido = "123.456.78";

            // Act & Assert
            var result = _validadorCPF.TestValidate(cpfInvalido);
            result.ShouldHaveValidationErrorFor(cpf => cpf).WithErrorMessage("O CPF está inválido");
        }

        [TestMethod]
        public void CPF_MaisDigitos_DeveRetornarErro()
        {
            // Arrange
            var cpfInvalido = "123.456.789-098";

            // Act & Assert
            var result = _validadorCPF.TestValidate(cpfInvalido);
            result.ShouldHaveValidationErrorFor(cpf => cpf).WithErrorMessage("O CPF está inválido");
        }
    }
}
