using FI.AtividadeEntrevista.Utils.Enumeradores;
using System;

namespace FI.AtividadeEntrevista.Utils
{
    public class Parametro
    {
        public Parametro(string nome, object value, ParametroDirecao direcao = ParametroDirecao.Entrada)
        {
            Nome = nome;
            Value = value;
            Tipo = value.GetType();
            Direcao = direcao;
        }

        public string Nome { get; set; }
        public object Value { get; set; }
        public Type Tipo { get; set; }
        public ParametroDirecao Direcao { get; set; }
    }
}
