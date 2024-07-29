using Dommel;
using System;

namespace FI.AtividadeEntrevista.Repositorio.Conventions
{
    public class TableNameResolver : ITableNameResolver
    {
        public string ResolveTableName(Type type)
        {
            return $"{type.Name}S".ToUpper();
        }
    }
}
