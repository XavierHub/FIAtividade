using FluentValidation.Results;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos
{
    public interface IServicoNotificacao
    {
        IReadOnlyCollection<Notificacao> Notificacoes();
        bool TemNotificacao();
        void Adicionar(string key, string message);
        void Adicionar(ValidationResult validationResult);
    }
}
