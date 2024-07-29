using EmpXpo.Accounting.Application.Services;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FI.AtividadeEntrevista.Tests.Services
{
    [TestClass]
    public class ServicoNotificacaoTests
    {
        private ServicoNotificacao _servicoNotificacao;

        [TestInitialize]
        public void Initialize()
        {
            _servicoNotificacao = new ServicoNotificacao();
        }

        [TestMethod]
        public void TemNotificacao_SemNotificacoes_DeveRetornarFalso()
        {
            // Act
            var result = _servicoNotificacao.TemNotificacao();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Adicionar_Notificacao_DeveTerNotificacao()
        {
            // Act
            _servicoNotificacao.Adicionar("Teste", "Mensagem de teste");
            var result = _servicoNotificacao.TemNotificacao();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Notificacoes_DeveRetornarNotificacoes()
        {
            // Arrange
            _servicoNotificacao.Adicionar("Teste", "Mensagem de teste");

            // Act
            var result = _servicoNotificacao.Notificacoes();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Teste", result.First().Chave);
            Assert.AreEqual("Mensagem de teste", result.First().Mensagem);
        }

        [TestMethod]
        public void Notificacoes_DeveLimparNotificacoesAposRetornar()
        {
            // Arrange
            _servicoNotificacao.Adicionar("Teste", "Mensagem de teste");

            // Act
            var result = _servicoNotificacao.Notificacoes();
            var resultAposLimpar = _servicoNotificacao.TemNotificacao();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsFalse(resultAposLimpar);
        }

        [TestMethod]
        public void Adicionar_ValidationResult_DeveAdicionarNotificacoes()
        {
            // Arrange
            var errors = new List<ValidationFailure>
            {
                new ValidationFailure("Campo1", "Erro no Campo1") { ErrorCode = "001" },
                new ValidationFailure("Campo2", "Erro no Campo2") { ErrorCode = "002" }
            };
            var validationResult = new ValidationResult(errors);

            // Act
            _servicoNotificacao.Adicionar(validationResult);
            var result = _servicoNotificacao.Notificacoes();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("001", result.ElementAt(0).Chave);
            Assert.AreEqual("Erro no Campo1", result.ElementAt(0).Mensagem);
            Assert.AreEqual("002", result.ElementAt(1).Chave);
            Assert.AreEqual("Erro no Campo2", result.ElementAt(1).Mensagem);
        }
    }
}
