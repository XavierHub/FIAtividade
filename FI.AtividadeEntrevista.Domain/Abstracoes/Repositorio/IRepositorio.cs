using FI.AtividadeEntrevista.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao.Repositorios
{
    public interface IRepositorio<T> where T : class
    {
        Task<IEnumerable<T>> Todos();
        Task<T> ObterPorId(object parm);
        Task<T> Obter(string spName, object parm = null);
        Task<object> Incluir(T entity);
        Task<bool> Excluir(T entity);
        Task<bool> Atualizar(T entity);
        Task<IEnumerable<T>> Consultar(Expression<Func<T, bool>> expression);
        //Task<IEnumerable<U>> Consultar<U>(string spName, object parm = null);
        Task<(IEnumerable<T>, U)> ConsultarMultSelecao<U>(string spName, object parmIn = null);
        Task<Resultado<IEnumerable<T>>> Consultar(string spName, List<Parametro> dbParametros);
        Task<U> ExecutarProcedure<U>(string spName, object parmIn = null);
    }
}
