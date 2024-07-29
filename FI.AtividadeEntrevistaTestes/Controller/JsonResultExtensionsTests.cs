using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FI.WebAtividadeEntrevista.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace FI.AtividadeEntrevista.Tests.Extensions
{
    [TestClass]
    public class JsonResultExtensionsTests
    {
        private Mock<IServicoNotificacao> _mockServicoNotificacao;
        private Mock<HttpResponseBase> _mockResponse;
        private JsonResult _jsonResult;

        [TestInitialize]
        public void Initialize()
        {
            _mockServicoNotificacao = new Mock<IServicoNotificacao>();
            _mockResponse = new Mock<HttpResponseBase>();
            _jsonResult = new JsonResult();
        }

        [TestMethod]
        public void ComNotificacoes_DeveAdicionarErrosNoJsonResult()
        {
            // Arrange
            var notificacoes = new List<Notificacao>
            {
                new Notificacao("Error1", "Mensagem de erro 1"),
                new Notificacao("Error2", "Mensagem de erro 2")
            };
            _mockServicoNotificacao.Setup(s => s.TemNotificacao()).Returns(true);
            _mockServicoNotificacao.Setup(s => s.Notificacoes()).Returns(notificacoes);

            // Act
            var result = _jsonResult.ComNotificacoes(_mockServicoNotificacao.Object, _mockResponse.Object);

            // Assert
            _mockResponse.VerifySet(r => r.StatusCode = 400);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Data, typeof(string));
            Assert.AreEqual("Mensagem de erro 1<br>Mensagem de erro 2", result.Data);
        }

        [TestMethod]
        public void ComNotificacoes_SemNotificacoes_NaoDeveModificarJsonResult()
        {
            // Arrange
            _mockServicoNotificacao.Setup(s => s.TemNotificacao()).Returns(false);

            // Act
            var result = _jsonResult.ComNotificacoes(_mockServicoNotificacao.Object, _mockResponse.Object);

            // Assert
            _mockResponse.VerifySet(r => r.StatusCode = It.IsAny<int>(), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public void ComModelStateErros_DeveAdicionarErrosNoJsonResult()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("Field1", "Erro no campo 1");
            modelState.AddModelError("Field2", "Erro no campo 2");

            // Act
            var result = _jsonResult.ComModelStateErros(modelState, _mockResponse.Object);

            // Assert
            _mockResponse.VerifySet(r => r.StatusCode = 400);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Data, typeof(string));
            Assert.AreEqual("Erro no campo 1<br>Erro no campo 2", result.Data);
        }

        [TestMethod]
        public void ComModelStateErros_SemErros_NaoDeveModificarJsonResult()
        {
            // Arrange
            var modelState = new ModelStateDictionary();

            // Act
            var result = _jsonResult.ComModelStateErros(modelState, _mockResponse.Object);

            // Assert
            _mockResponse.VerifySet(r => r.StatusCode = It.IsAny<int>(), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
        }
    }
}
