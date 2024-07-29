using EmpXpo.Accounting.Application.Services.Validators;
using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FI.AtividadeEntrevista.Dominio.Enumeradores;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Tests.Services.Validators
{
    [TestClass]
    public class ServicoValidacaoBeneficiarioTests
    {
        private Mock<IServicoNotificacao> _mockServicoNotificacao;
        private ServicoValidacaoBeneficiario _servicoValidacaoBeneficiario;

        [TestInitialize]
        public void Initialize()
        {
            _mockServicoNotificacao = new Mock<IServicoNotificacao>();
            _servicoValidacaoBeneficiario = new ServicoValidacaoBeneficiario(_mockServicoNotificacao.Object);
        }

        [TestMethod]
        public async Task Validar_ModelNulo_DeveRetornarFalso()
        {
            // Act
            var result = await _servicoValidacaoBeneficiario.Validar(TipoValidacao.Inserir, null);

            // Assert
            Assert.IsFalse(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }

        [TestMethod]
        public async Task Validar_ModelValido_DeveRetornarVerdadeiro()
        {
            // Arrange
            var beneficiario = new Beneficiario { Nome = "Nome Teste", CPF = "123.456.789-09" };

            // Act
            var result = await _servicoValidacaoBeneficiario.Validar(TipoValidacao.Inserir, beneficiario);

            // Assert
            Assert.IsTrue(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }

        [TestMethod]
        public async Task Validar_ModelInvalido_DeveRetornarFalso()
        {
            // Arrange
            var beneficiario = new Beneficiario { Nome = "" };

            // Act
            var result = await _servicoValidacaoBeneficiario.Validar(TipoValidacao.Inserir, beneficiario);

            // Assert
            Assert.IsFalse(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task ValorValido_ValorInvalido_DeveRetornarFalso()
        {
            // Act
            var result = await _servicoValidacaoBeneficiario.ValorValido("Nome", "", value => !string.IsNullOrWhiteSpace(value), "O Nome é obrigatório");

            // Assert
            Assert.IsFalse(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }

        [TestMethod]
        public async Task ValorValido_ValorValido_DeveRetornarVerdadeiro()
        {
            // Act
            var result = await _servicoValidacaoBeneficiario.ValorValido("Nome", "Nome Teste", value => !string.IsNullOrWhiteSpace(value), "O Nome é obrigatório");

            // Assert
            Assert.IsTrue(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }

        [TestMethod]
        public async Task CPFValido_CPFInvalido_DeveRetornarFalso()
        {
            // Act
            var result = await _servicoValidacaoBeneficiario.CPFValido("123");

            // Assert
            Assert.IsFalse(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }

        [TestMethod]
        public async Task CPFValido_CPFValido_DeveRetornarVerdadeiro()
        {
            // Act
            var result = await _servicoValidacaoBeneficiario.CPFValido("123.456.789-09");

            // Assert
            Assert.IsTrue(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }
    }
}
