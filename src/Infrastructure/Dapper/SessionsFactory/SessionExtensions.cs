namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.SessionsFactory
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;

    public static class SessionExtensions
    {
        public static IAsyncEnumerable<TSource> Query<TSource>(
            this ISession session,
            QueryObject queryObject,
            SqlExecutionOptions executionOptions,
            CancellationToken cancellationToken = default)
        {
            return session.Query<TSource>(
                queryObject.Sql,
                queryObject.QueryParams,
                executionOptions.CommandTimeoutSeconds,
                cancellationToken: cancellationToken);
        }

        public static Task<IEnumerable<TSource>> QueryAsync<TSource>(
            this ISession session,
            QueryObject queryObject,
            SqlExecutionOptions executionOptions,
            CancellationToken cancellationToken = default)
        {
            return session.QueryAsync<TSource>(
                queryObject.Sql,
                queryObject.QueryParams,
                executionOptions.CommandTimeoutSeconds,
                cancellationToken: cancellationToken);
        }

        public static Task<int> ExecuteAsync(
            this ISession session,
            QueryObject queryObject,
            SqlExecutionOptions executionOptions,
            CancellationToken cancellationToken = default)
        {
            return session.ExecuteAsync(
                queryObject.Sql,
                queryObject.QueryParams,
                executionOptions.CommandTimeoutSeconds,
                cancellationToken: cancellationToken);
        }

        public static Task<TSource> ExecuteScalarAsync<TSource>(
            this ISession session,
            QueryObject queryObject,
            SqlExecutionOptions executionOptions,
            CancellationToken cancellationToken = default)
        {
            return session.ExecuteScalarAsync<TSource>(
                queryObject.Sql,
                queryObject.QueryParams,
                executionOptions.CommandTimeoutSeconds,
                cancellationToken: cancellationToken);
        }
    }
}