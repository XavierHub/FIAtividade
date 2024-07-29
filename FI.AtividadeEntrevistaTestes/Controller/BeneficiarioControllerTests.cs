using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using WebAtividadeEntrevista.Controllers;

namespace FI.AtividadeEntrevistaTestes
{

    [TestClass]
    public class BeneficiarioControllerTests
    {
        private readonly BeneficiarioController _controller;

        public BeneficiarioControllerTests()
        {
            _controller = new BeneficiarioController();
        }


        [TestMethod]
        public void TestMethod1()
        {
            // Act
            var result = _controller.Index() as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("_Index", result.ViewName);
        }
    }
}
