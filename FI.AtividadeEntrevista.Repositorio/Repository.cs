using Dapper;
using Dommel;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Repositorio
{
    public class Repository<T> : IRepositorio<T> where T : class
    {
        private readonly string _connectionString;

        public Repository(RepositoryOptions options)
        {
            _connectionString = options.ConnectionString ?? throw new ArgumentException("Connection string is empty", nameof(options.ConnectionString));

        }

        public async Task<T> ObterPorId(object parm)
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                return await connection.GetAsync<T>(parm) ?? Activator.CreateInstance<T>();
            }
        }

        public async Task<IEnumerable<T>> Consultar(Expression<Func<T, bool>> expression)
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                return await connection.SelectAsync(expression);
            }
        }

        public async Task<(IEnumerable<T>, U)> ConsultarMultSelecao<U>(string spName, object parmIn = null)
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                using (var multi = await connection.QueryMultipleAsync(spName, parmIn, commandType: CommandType.StoredProcedure))
                {
                    var dados1 = await multi.ReadAsync<T>();
                    var dados2 = await multi.ReadFirstOrDefaultAsync<U>();

                    return (dados1, dados2);
                }
            }
        }

        public async Task<U> ExecutarProcedure<U>(string spName, object parmIn = null)
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<U>(spName,
                                                            param: parmIn,
                                                            commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault();

            }
        }

        private SqlConnection ObterConexao()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
