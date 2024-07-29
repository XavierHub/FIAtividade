namespace FI.AtividadeEntrevista.Dominio
{
    public class Notificacao
    {
        public string Chave { get; }
        public string Mensagem { get; }

        public Notificacao(string key, string message)
        {
            Chave = key;
            Mensagem = message;
        }
    }
}
