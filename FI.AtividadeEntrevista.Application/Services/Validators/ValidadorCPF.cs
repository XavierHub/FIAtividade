﻿using FluentValidation;
using System.Linq;
using System.Text.RegularExpressions;

namespace FI.AtividadeEntrevista.Aplicacao.Services.Validators
{
    public class ValidadorCPF : AbstractValidator<string>
    {
        public ValidadorCPF(string errorMessage)
        {
            RuleFor(cpf => cpf)
                .NotEmpty().WithMessage("O CPF é obrigatório")
                .Must(BeAValidCPF).WithMessage(errorMessage);
        }

        private bool BeAValidCPF(string cpf)
        {
            // Remove non-numeric characters
            cpf = Regex.Replace(cpf, "[^0-9]", "");

            if (cpf.Length != 11)
                return false;

            // Check for repeated sequences
            var repeatedSequences = new[] {
            "00000000000", "11111111111", "22222222222", "33333333333",
            "44444444444", "55555555555", "66666666666", "77777777777",
            "88888888888", "99999999999"
        };
            if (repeatedSequences.Contains(cpf))
                return false;

            // Compute verification digits
            int[] multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

            int remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            string digit = remainder.ToString();
            tempCpf += digit;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

            remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            digit += remainder.ToString();

            return cpf.EndsWith(digit);
        }
    }
}
