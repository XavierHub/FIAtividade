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
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Tests.Aplicacao
{
    [TestClass]
    public class AplicacaoDoClienteTests
    {
        private Mock<IRepositorio<Cliente>> _mockClienteRepositorio;
        private Mock<IServicoValidacao<Cliente>> _mockServicoValidacao;
        private Mock<IServicoNotificacao> _mockServicoNotificacao;
        private AplicacaoDoCliente _aplicacaoDoCliente;

        [TestInitialize]
        public void Initialize()
        {
            _mockClienteRepositorio = new Mock<IRepositorio<Cliente>>();
            _mockServicoValidacao = new Mock<IServicoValidacao<Cliente>>();
            _mockServicoNotificacao = new Mock<IServicoNotificacao>();
            _aplicacaoDoCliente = new AplicacaoDoCliente(
                _mockClienteRepositorio.Object,
                _mockServicoValidacao.Object,
                _mockServicoNotificacao.Object
            );
        }

        [TestMethod]
        public async Task ObterPorId_IdInvalido_DeveRetornarClienteVazio()
        {
            // Arrange
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<object, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(false);

            // Act
            var result = await _aplicacaoDoCliente.ObterPorId(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
        }

        [TestMethod]
        public async Task ObterPorId_IdValido_DeveRetornarCliente()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Teste" };
            
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<long, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);

            _mockClienteRepositorio.Setup(x => x.ExecutarProcedure<Cliente>("FI_SP_ConsCliente", It.IsAny<object>()))
                                   .ReturnsAsync(cliente);

            // Act
            var result = await _aplicacaoDoCliente.ObterPorId(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Teste", result.Nome);
        }


        [TestMethod]
        public async Task Inserir_CPFExistente_DeveRetornarClienteVazio()
        {
            // Arrange
            var cliente = new Cliente { CPF = "123.456.789-00" };
            _mockServicoValidacao.Setup(x => x.Validar(It.IsAny<TipoValidacao>(), It.IsAny<Cliente>()))
                                 .ReturnsAsync(true);
            _mockClienteRepositorio.Setup(x => x.ExecutarProcedure<bool>("FI_SP_VerificaCliente", It.IsAny<object>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _aplicacaoDoCliente.Inserir(cliente);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
            _mockServicoNotificacao.Verify(x => x.Adicionar("CPF", $"O CPF {cliente.CPF} já está cadastrado."), Times.Once);
        }

        [TestMethod]
        public async Task Inserir_ClienteValido_DeveRetornarClienteComId()
        {
            // Arrange
            var cliente = new Cliente { CPF = "123.456.789-00" };
            _mockServicoValidacao.Setup(x => x.Validar(It.IsAny<TipoValidacao>(), It.IsAny<Cliente>()))
                                 .ReturnsAsync(true);
            _mockClienteRepositorio.Setup(x => x.ExecutarProcedure<bool>("FI_SP_VerificaCliente", It.IsAny<object>()))
                                   .ReturnsAsync(false);
            _mockClienteRepositorio.Setup(x => x.ExecutarProcedure<long>("FI_SP_IncClienteV2", It.IsAny<object>()))
                                   .ReturnsAsync(1L);

            // Act
            var result = await _aplicacaoDoCliente.Inserir(cliente);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task Alterar_IdInvalido_DeveRetornarFalso()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Teste" };
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<object, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(false);

            // Act
            var result = await _aplicacaoDoCliente.Alterar(1, cliente);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Alterar_ClienteValido_DeveRetornarVerdadeiro()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Teste" };
            _mockServicoValidacao.Setup(x => x.Validar(It.IsAny<TipoValidacao>(), It.IsAny<Cliente>()))
                                 .ReturnsAsync(true);
            _mockServicoValidacao.Setup(x => x.ValorValido(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Func<long, bool>>(), It.IsAny<string>()))
                                 .ReturnsAsync(true);
            _mockClienteRepositorio.Setup(x => x.ObterPorId(It.IsAny<long>())).ReturnsAsync(cliente);

            // Act
            var result = await _aplicacaoDoCliente.Alterar(1, cliente);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Pesquisar_DeveRetornarListaDeClientes()
        {
            // Arrange
            var clientes = new List<Cliente> { new Cliente { Id = 1, Nome = "Teste" } };
            _mockClienteRepositorio.Setup(x => x.ConsultarMultSelecao<int>("FI_SP_PesqCliente", It.IsAny<object>()))
                                   .ReturnsAsync((clientes.AsEnumerable(), 1));

            // Act
            var (resultado, total) = await _aplicacaoDoCliente.Pesquisar(0, 10, "Nome", true);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, total);
            Assert.AreEqual(1, resultado.Count());
        }
    }
}
