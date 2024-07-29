using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao.Repositorios
{
    public interface IRepositorio<T> where T : class
    {
        Task<T> ObterPorId(object parm);
        Task<IEnumerable<T>> Consultar(Expression<Func<T, bool>> expression);
        Task<(IEnumerable<T>, U)> ConsultarMultSelecao<U>(string spName, object parmIn = null);
        Task<U> ExecutarProcedure<U>(string spName, object parmIn = null);
    }
}
