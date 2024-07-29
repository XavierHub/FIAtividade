using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace EmpXpo.Accounting.Application.Services
{
    public class ServicoNotificacao : IServicoNotificacao
    {
        private readonly List<Notificacao> _notificacoes;

        public ServicoNotificacao()
        {
            _notificacoes = new List<Notificacao>();
        }
        public bool TemNotificacao() => _notificacoes.Any();
        public void Adicionar(string key, string message) => _notificacoes.Add(new Notificacao(key, message));

        public IReadOnlyCollection<Notificacao> Notificacoes()
        {
            var notificacoesAtuais = new List<Notificacao>(_notificacoes);
            _notificacoes.Clear();
            return notificacoesAtuais;
        }

        public void Adicionar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Adicionar(error.ErrorCode, error.ErrorMessage);
            }
        }
    }
}
