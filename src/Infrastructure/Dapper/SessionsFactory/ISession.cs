namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.SessionsFactory
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public interface ISession : IDisposable 
    {
        IEnumerable<TSource> Query<TSource>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
        Task<IEnumerable<TSource>> QueryAsync<TSource>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        void Commit();
    }
}