namespace FI.AtividadeEntrevista.Dominio
{
    public class Beneficiario : EntityBase
    {
        public long IdCliente { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
    }
}
