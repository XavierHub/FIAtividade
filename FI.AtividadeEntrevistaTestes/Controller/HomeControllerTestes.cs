using FI.WebAtividadeEntrevista.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace FI.WebAtividadeEntrevista.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new HomeController();
        }

        [TestMethod]
        public void Index_DeveRetornarView()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName); // Verifica se a View retornada é a padrão (Index)
        }

        [TestMethod]
        public void About_DeveRetornarViewComMensagem()
        {
            // Act
            var result = _controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Your application description page.", _controller.ViewBag.Message);
            Assert.AreEqual("", result.ViewName); // Verifica se a View retornada é a padrão (About)
        }

        [TestMethod]
        public void Contact_DeveRetornarViewComMensagem()
        {
            // Act
            var result = _controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Your contact page.", _controller.ViewBag.Message);
            Assert.AreEqual("", result.ViewName); // Verifica se a View retornada é a padrão (Contact)
        }
    }
}
