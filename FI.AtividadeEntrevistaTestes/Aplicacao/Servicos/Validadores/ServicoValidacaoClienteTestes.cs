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
    public class ServicoValidacaoClienteTestes
    {
        private Mock<IServicoNotificacao> _mockServicoNotificacao;
        private ServicoValidacaoCliente _servicoValidacaoCliente;

        [TestInitialize]
        public void Initialize()
        {
            _mockServicoNotificacao = new Mock<IServicoNotificacao>();
            _servicoValidacaoCliente = new ServicoValidacaoCliente(_mockServicoNotificacao.Object);
        }

        [TestMethod]
        public async Task Validar_ModelNulo_DeveRetornarFalso()
        {
            // Act
            var result = await _servicoValidacaoCliente.Validar(TipoValidacao.Inserir, null);

            // Assert
            Assert.IsFalse(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }

        [TestMethod]
        public async Task Validar_ModelValido_DeveRetornarVerdadeiro()
        {
            // Arrange
            var cliente = new Cliente { CPF = "123.456.789-09", Nome = "Nome Teste", Email = "email@teste.com" };

            // Act
            var result = await _servicoValidacaoCliente.Validar(TipoValidacao.Inserir, cliente);

            // Assert
            Assert.IsTrue(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.AtLeast(2));
        }

        [TestMethod]
        public async Task Validar_ModelInvalido_DeveRetornarFalso()
        {
            // Arrange
            var cliente = new Cliente { CPF = "", Nome = "", Email = "emailinvalido" };

            // Act
            var result = await _servicoValidacaoCliente.Validar(TipoValidacao.Inserir, cliente);

            // Assert
            Assert.IsFalse(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task ValorValido_ValorInvalido_DeveRetornarFalso()
        {
            // Act
            var result = await _servicoValidacaoCliente.ValorValido("CEP", "123", value => value.Length == 8, "O CEP deve ter 8 dígitos");

            // Assert
            Assert.IsFalse(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }

        [TestMethod]
        public async Task ValorValido_ValorValido_DeveRetornarVerdadeiro()
        {
            // Act
            var result = await _servicoValidacaoCliente.ValorValido("CEP", "12345678", value => value.Length == 8, "O CEP deve ter 8 dígitos");

            // Assert
            Assert.IsTrue(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }

        [TestMethod]
        public async Task CPFValido_CPFInvalido_DeveRetornarFalso()
        {
            // Act
            var result = await _servicoValidacaoCliente.CPFValido("123");

            // Assert
            Assert.IsFalse(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }

        [TestMethod]
        public async Task CPFValido_CPFValido_DeveRetornarVerdadeiro()
        {
            // Act
            var result = await _servicoValidacaoCliente.CPFValido("123.456.789-09");

            // Assert
            Assert.IsTrue(result);
            _mockServicoNotificacao.Verify(x => x.Adicionar(It.IsAny<ValidationResult>()), Times.Once);
        }
    }
}
