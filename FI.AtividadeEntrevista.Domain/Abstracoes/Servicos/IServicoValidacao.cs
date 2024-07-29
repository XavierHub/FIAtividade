using FI.AtividadeEntrevista.Dominio.Enumeradores;
using System;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos
{
    public interface IServicoValidacao<T> where T : class
    {
        Task<bool> Validar(TipoValidacao tipoValidacao, T model);
        Task<bool> ValorValido<TValue>(string name, TValue value, Func<TValue, bool> regra, string mensagem = "The '{PropertyName}' has an invalid value");
        Task<bool> CPFValido(string value, string errorMessage = "O CPF está inválido");
    }
}
