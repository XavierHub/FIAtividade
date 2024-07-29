using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAtividadeEntrevista.Controllers;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Tests.Controllers
{
    [TestClass]
    public class ClienteControllerTests
    {
        private Mock<IAplicacaoDoCliente> _mockAplicacaoDoCliente;
        private Mock<IAplicacaoDoBeneficiario> _mockAplicacaoDoBeneficiario;
        private Mock<IServicoNotificacao> _mockServicoNotificacao;
        private Mock<HttpResponseBase> _mockHttpResponse;
        private ClienteController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _mockAplicacaoDoCliente = new Mock<IAplicacaoDoCliente>();
            _mockAplicacaoDoBeneficiario = new Mock<IAplicacaoDoBeneficiario>();
            _mockServicoNotificacao = new Mock<IServicoNotificacao>();
            _mockHttpResponse = new Mock<HttpResponseBase>();

            _controller = new ClienteController(
                _mockAplicacaoDoCliente.Object,
                _mockAplicacaoDoBeneficiario.Object,
                _mockServicoNotificacao.Object
            );

            // Configurar o mock do HttpContext e HttpResponse
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Response).Returns(_mockHttpResponse.Object);
            _controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new System.Web.Routing.RouteData(), _controller);
        }

        [TestMethod]
        public async Task Incluir_ModelInvalido_DeveRetornarErrosDeModelState()
        {
            // Arrange
            _controller.ModelState.AddModelError("Nome", "Nome é obrigatório");
            var model = new ClienteModel();

            // Act
            var result = await _controller.Incluir(model) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            _mockHttpResponse.VerifySet(r => r.StatusCode = 400, Times.Once);
            Assert.AreEqual("Nome é obrigatório", result.Data.ToString());
        }

        [TestMethod]
        public async Task Incluir_ModelValido_DeveRetornarSucesso()
        {
            // Arrange
            var model = new ClienteModel
            {
                CEP = "12345-678",
                Cidade = "Cidade",
                Email = "email@teste.com",
                Estado = "SP",
                Logradouro = "Logradouro",
                Nacionalidade = "Brasileiro",
                Nome = "Nome",
                Sobrenome = "Sobrenome",
                CPF = "123.456.789-00",
                Telefone = "12345-6789",
                Beneficiarios = new BeneficiarioModel[]
                {
                    new BeneficiarioModel
                    {
                        IdCliente = 1,
                        Nome = "Beneficiario",
                        CPF = "123.456.789-01"
                    }
                }
            };
            _mockAplicacaoDoCliente.Setup(x => x.Inserir(It.IsAny<Cliente>())).ReturnsAsync(new Cliente { Id = 1 });

            // Act
            var result = await _controller.Incluir(model) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Cadastro efetuado com sucesso", result.Data);
        }

        [TestMethod]
        public async Task Alterar_ModelInvalido_DeveRetornarErrosDeModelState()
        {
            // Arrange
            _controller.ModelState.AddModelError("Nome", "Nome é obrigatório");
            var model = new ClienteModel();

            // Act
            var result = await _controller.Alterar(model) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            _mockHttpResponse.VerifySet(r => r.StatusCode = 400, Times.Once);
            Assert.AreEqual("Nome é obrigatório", result.Data.ToString());
        }

        [TestMethod]
        public async Task Alterar_ModelValido_DeveRetornarSucesso()
        {
            // Arrange
            var model = new ClienteModel
            {
                Id = 1,
                CEP = "12345-678",
                Cidade = "Cidade",
                Email = "email@teste.com",
                Estado = "SP",
                Logradouro = "Logradouro",
                Nacionalidade = "Brasileiro",
                Nome = "Nome",
                Sobrenome = "Sobrenome",
                CPF = "123.456.789-00",
                Telefone = "12345-6789",
                Beneficiarios = new BeneficiarioModel[]
                {
                    new BeneficiarioModel
                    {
                        IdCliente = 1,
                        Nome = "Beneficiario",
                        CPF = "123.456.789-01"
                    }
                }
            };

            // Act
            var result = await _controller.Alterar(model) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Cadastro alterado com sucesso", result.Data);
        }

        [TestMethod]
        public async Task ClienteList_DeveRetornarClientes()
        {
            // Arrange
            var clientes = new List<Cliente> { new Cliente { Id = 1, Nome = "Nome" } };
            _mockAplicacaoDoCliente.Setup(x => x.Pesquisar(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync((clientes.AsEnumerable(), 1));

            // Act
            var result = await _controller.ClienteList(0, 10, "Nome ASC") as JsonResult;

            // Assert
            Assert.IsNotNull(result);

            var data = result.Data;
            Assert.AreEqual("OK", data.Get<string>("Result"));
            Assert.AreEqual(1, data.Get<IEnumerable<Cliente>>("Records").Count());
            Assert.AreEqual(1, data.Get<int>("TotalRecordCount"));
        }

        [TestMethod]
        public async Task ListarBeneficiarios_ComIdCliente_DeveRetornarBeneficiarios()
        {
            // Arrange
            var beneficiarios = new List<Beneficiario>
            {
                new Beneficiario { Id = 1, IdCliente = 1, Nome = "Beneficiario", CPF = "123.456.789-01" }
            };
            _mockAplicacaoDoBeneficiario.Setup(x => x.ConsultarPorIdCliente(1)).ReturnsAsync(beneficiarios);

            // Act
            var result = await _controller.ListarBeneficiarios(1) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            var data = result.Data as List<BeneficiarioModel>;
            Assert.AreEqual(1, data.Count);
        }

        [TestMethod]
        public async Task ListarBeneficiarios_SemIdCliente_DeveRetornarListaVazia()
        {
            // Act
            var result = await _controller.ListarBeneficiarios(null) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            var data = result.Data as List<BeneficiarioModel>;
            Assert.AreEqual(0, data.Count);
        }
    }
}
