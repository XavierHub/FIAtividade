
namespace FI.AtividadeEntrevista.Dominio.Enumeradores
{
    public class TipoValidacao
    {
        private readonly string _nome;
        public string Name => _nome;

        public static readonly TipoValidacao Inserir = new TipoValidacao("Inserir");
        public static readonly TipoValidacao Alterar = new TipoValidacao("Alterar");
        public static readonly TipoValidacao Excluir = new TipoValidacao("Excluir");
        public static readonly TipoValidacao Obter = new TipoValidacao("Obter");

        public TipoValidacao(string nome)
        {
            _nome = nome;
        }

    }
}
