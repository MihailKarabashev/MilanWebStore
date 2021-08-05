namespace MilanWebStore.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;

    public interface IDbQueryRunner : IDisposable
    {
        Task RunQueryAsync(string query, params object[] parameters);

        public List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map);
    }
}
