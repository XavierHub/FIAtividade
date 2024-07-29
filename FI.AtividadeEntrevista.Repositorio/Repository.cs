using Dapper;
using Dommel;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao.Repositorios;
using FI.AtividadeEntrevista.Utils;
using FI.AtividadeEntrevista.Utils.Enumeradores;
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

        public async Task<IEnumerable<T>> Todos()
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                return await connection.GetAllAsync<T>();
            }
        }

        public async Task<T> ObterPorId(object parm)
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                return await connection.GetAsync<T>(parm) ?? Activator.CreateInstance<T>();
            }
        }

        public async Task<T> Obter(string spName, object parm = null)
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<T>(spName,
                                                            param: parm,
                                                            commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault() ?? Activator.CreateInstance<T>();
            }
        }

        public async Task<bool> Excluir(T entity)
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                return await connection.DeleteAsync(entity);
            }
        }

        public async Task<bool> Atualizar(T entity)
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                return await connection.UpdateAsync<T>(entity);
            }
        }

        public async Task<object> Incluir(T entity)
        {
            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                return await connection.InsertAsync(entity);
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

        public async Task<Resultado<IEnumerable<T>>> Consultar(string spName, List<Parametro> parametros)
        {
            var parameters = ObterParametros(parametros);
            var resultado = new Resultado<IEnumerable<T>>();

            using (var connection = ObterConexao())
            {
                await connection.OpenAsync();
                resultado.Dados = await connection.QueryAsync<T>(spName,
                                                        param: parameters,
                                                        commandType: CommandType.StoredProcedure);
            }

            resultado.Parametros = ObterValoresDeSaida(parametros, parameters);

            return resultado;
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

        private List<Parametro> ObterValoresDeSaida(List<Parametro> parametros, DynamicParameters parameters)
        {
            foreach (var parm in parametros.Where(x => x.Direcao == ParametroDirecao.Saida))
            {
                parm.Value = parameters.Get<dynamic>(parm.Nome);
            }
            return parametros;
        }

        private DynamicParameters ObterParametros(List<Parametro> dbParametros)
        {
            var parameters = new DynamicParameters();
            dbParametros.ForEach(parm =>
                parameters.Add(parm.Nome,
                               parm.Value,
                               direction: parm.Direcao == ParametroDirecao.Entrada ?
                                                          ParameterDirection.Input : ParameterDirection.Output
                              )
            );
            return parameters;
        }

        public async Task<U> ExecutarProcedure<U>(string spName, object parmIn = null)
        {
            try
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
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SqlConnection ObterConexao()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
