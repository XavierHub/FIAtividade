using System.Collections.Generic;

namespace FI.AtividadeEntrevista.Dominio
{
    public class Cliente : EntityBase
    {
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
        public string Logradouro { get; set; }
        public string Nacionalidade { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }

        public List<Beneficiario> Beneficiarios { get; set; }
    }
}
