using FI.AtividadeEntrevista.Application;
using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao.Repositorios;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FI.AtividadeEntrevista.Dominio.Enumeradores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Tests.Aplicacao
{
    [TestClass]
    public class AplicacaoDoBeneficiarioTests
    {
        private Mock<IRepositorio<Beneficiario>> _mockBeneficiarioRepositorio;
        private Mock<IServicoValidacao<Beneficiario>> _mockServicoValidacao;
        private Mock<IServicoNotificacao> _mockServicoNotificacao;
        private AplicacaoDoBeneficiario _aplicacaoDoBeneficiario;

        [TestInitialize]
        public void Initialize()
        {
            _mockBeneficiarioRepositorio = new Mock<IRepositorio<Beneficiario>>();
            _mockServicoValidacao = new Mock<IServicoValidacao<Beneficiario>>();
            _mockServicoNotificacao = new Mock<IServicoNotificacao>();
            _aplicacaoDoBeneficiario = new AplicacaoDoBeneficiario(
                _mockBeneficiarioRepositorio.Object,
                _mockServicoValidacao.Object,
                _mockServicoNotificacao.Object
            );
        }

        [TestMethod]
        public async Task ConsultarPorIdCliente_IdInvalido_DeveRetornarListaVazia()
        {
            // Arrange
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<object, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(false);

            // Act
            var result = await _aplicacaoDoBeneficiario.ConsultarPorIdCliente(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task ConsultarPorIdCliente_IdValido_DeveRetornarListaDeBeneficiarios()
        {
            // Arrange
            var beneficiarios = new List<Beneficiario>
            {
                new Beneficiario { Id = 1, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<long, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockBeneficiarioRepositorio.Setup(x => x.Consultar(It.IsAny<Expression<Func<Beneficiario, bool>>>()))
                                        .ReturnsAsync(beneficiarios);

            // Act
            var result = await _aplicacaoDoBeneficiario.ConsultarPorIdCliente(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task Alterar_IdClienteInvalido_DeveRetornarFalso()
        {
            // Arrange
            var beneficiarios = new List<Beneficiario>
            {
                new Beneficiario { Id = 1, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<object, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(false);

            // Act
            var result = await _aplicacaoDoBeneficiario.Alterar(1, beneficiarios);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Alterar_BeneficiarioInvalido_DeveRetornarFalso()
        {
            // Arrange
            var beneficiarios = new List<Beneficiario>
            {
                new Beneficiario { Id = 1, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<object, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.Validar(It.IsAny<TipoValidacao>(), It.IsAny<Beneficiario>()))
                                 .ReturnsAsync(false);

            // Act
            var result = await _aplicacaoDoBeneficiario.Alterar(1, beneficiarios);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Alterar_CPFInvalido_DeveRetornarFalso()
        {
            // Arrange
            var beneficiarios = new List<Beneficiario>
            {
                new Beneficiario { Id = 1, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<object, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.Validar(It.IsAny<TipoValidacao>(), It.IsAny<Beneficiario>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.CPFValido(It.IsAny<string>(), It.IsAny<string>()))
                                 .ReturnsAsync(false);

            // Act
            var result = await _aplicacaoDoBeneficiario.Alterar(1, beneficiarios);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Alterar_BeneficiariosValidos_DeveRetornarVerdadeiro()
        {
            // Arrange
            var beneficiarios = new List<Beneficiario>
            {
                new Beneficiario { Id = 1, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<long, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.Validar(It.IsAny<TipoValidacao>(), It.IsAny<Beneficiario>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.CPFValido(It.IsAny<string>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);

            // Act
            var result = await _aplicacaoDoBeneficiario.Alterar(1, beneficiarios);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Alterar_ExcluirBeneficiarios_DeveChamarExecutarProcedure()
        {
            // Arrange
            var beneficiarios = new List<Beneficiario>
            {
                new Beneficiario { Id = 1, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            var existentes = new List<Beneficiario>
            {
                new Beneficiario { Id = 2, IdCliente = 1, Nome = "Outro Beneficiario", CPF = "987.654.321-00" }
            };
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<long, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.Validar(It.IsAny<TipoValidacao>(), It.IsAny<Beneficiario>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.CPFValido(It.IsAny<string>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockBeneficiarioRepositorio.Setup(x => x.Consultar(It.IsAny<Expression<Func<Beneficiario, bool>>>()))
                                        .ReturnsAsync(existentes);

            // Act
            var result = await _aplicacaoDoBeneficiario.Alterar(1, beneficiarios);

            // Assert
            Assert.IsTrue(result);
            _mockBeneficiarioRepositorio.Verify(x => x.ExecutarProcedure<long>("FI_SP_DelBeneficiario", It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public async Task Alterar_InserirBeneficiarios_DeveChamarExecutarProcedure()
        {
            // Arrange
            var beneficiarios = new List<Beneficiario>
            {
                new Beneficiario { Id = 0, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            var existentes = new List<Beneficiario>();
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<long, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.Validar(It.IsAny<TipoValidacao>(), It.IsAny<Beneficiario>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.CPFValido(It.IsAny<string>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockBeneficiarioRepositorio.Setup(x => x.Consultar(It.IsAny<Expression<Func<Beneficiario, bool>>>()))
                                        .ReturnsAsync(existentes);

            // Act
            var result = await _aplicacaoDoBeneficiario.Alterar(1, beneficiarios);

            // Assert
            Assert.IsTrue(result);
            _mockBeneficiarioRepositorio.Verify(x => x.ExecutarProcedure<long>("FI_SP_IncBeneficiario", It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public async Task Alterar_AtualizarBeneficiarios_DeveChamarExecutarProcedure()
        {
            // Arrange
            var beneficiarios = new List<Beneficiario>
            {
                new Beneficiario { Id = 1, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            var existentes = new List<Beneficiario>
            {
                new Beneficiario { Id = 1, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<long, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.Validar(It.IsAny<TipoValidacao>(), It.IsAny<Beneficiario>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.CPFValido(It.IsAny<string>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockBeneficiarioRepositorio.Setup(x => x.Consultar(It.IsAny<Expression<Func<Beneficiario, bool>>>()))
                                        .ReturnsAsync(existentes);

            // Act
            var result = await _aplicacaoDoBeneficiario.Alterar(1, beneficiarios);

            // Assert
            Assert.IsTrue(result);
            _mockBeneficiarioRepositorio.Verify(x => x.ExecutarProcedure<long>("FI_SP_AltBeneficiario", It.IsAny<object>()), Times.Once);
        }
    }
}
