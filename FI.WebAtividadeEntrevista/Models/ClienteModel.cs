﻿using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Cliente
    /// </summary>
    public class ClienteModel
    {
        public long? Id { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        [Required]
        [MaxLength(9)]
        public string CEP { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Cidade { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
        [MaxLength(500)]
        public string Email { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string Estado { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Logradouro { get; set; }

        /// <summary>
        /// Nacionalidade
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Sobrenome { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        [MaxLength(15)]
        public string Telefone { get; set; }

        /// <summary>
        /// Cpf
        /// </summary>        
        [Required]
        [MaxLength(14)]
        public string CPF { get; set; }

        public BeneficiarioModel[] Beneficiarios { get; set; }
    }
}