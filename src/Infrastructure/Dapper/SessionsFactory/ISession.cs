namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.SessionsFactory
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISession : IDisposable 
    {
        IAsyncEnumerable<TSource> Query<TSource>(
            string sql,
            object? param = null,
            int? commandTimeout = null,
            CommandType? commandType = null,
            CancellationToken cancellationToken = default);


        Task<IEnumerable<TSource>> QueryAsync<TSource>(
            string sql, 
            object? param = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null,
            CancellationToken cancellationToken = default);

        Task<int> ExecuteAsync(
            string sql, 
            object? param = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null,
            CancellationToken cancellationToken = default);

        Task<TSource> ExecuteScalarAsync<TSource>(
            string sql, 
            object? param = null, 
            int? commandTimeout = null,
            CommandType? commandType = null,
            CancellationToken cancellationToken = default);

        void Commit();
    }
}