using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Cliente
    /// </summary>
    public class BeneficiarioModel
    {
        public long? Id { get; set; }

        public long IdCliente { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nome { get; set; }

        /// <summary>
        /// Cpf
        /// </summary>        
        [Required]
        [MaxLength(14)]
        public string CPF { get; set; }
    }
}