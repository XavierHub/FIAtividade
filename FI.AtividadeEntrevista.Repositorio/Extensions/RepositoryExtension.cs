using Dommel;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao.Repositorios;
using FI.AtividadeEntrevista.Repositorio.Conventions;
using System;
using System.Configuration;
using System.Diagnostics;
using Unity;
using Unity.Lifetime;

namespace FI.AtividadeEntrevista.Repositorio
{
    public static class RepositoryExtension
    {
        public static void AddRepository(this IUnityContainer container)
        {
            // Obter a string de conexão diretamente do ConfigurationManager
            var connectionString = ConfigurationManager.ConnectionStrings["BancoDeDados"].ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string is empty", nameof(connectionString));

            // Registrar opções de repositório
            container.RegisterInstance(new RepositoryOptions
            {
                ConnectionString = connectionString
            });

            // Registrar o repositório genérico
            container.RegisterType(typeof(IRepositorio<>), typeof(Repository<>), new HierarchicalLifetimeManager());

            DommelMapper.LogReceived = (e) => Debug.WriteLine(e);
            DommelMapper.SetTableNameResolver(new TableNameResolver());
        }
    }
}
