namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.Extensions
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Dapper;

    public static class DbConnectionExtensions
    {
        public static async IAsyncEnumerable<TSource> Query<TSource>(
            this DbConnection connection,
            QueryObject queryObject,
            SqlExecutionOptions? executionOptions = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(queryObject.Sql, queryObject.QueryParams, null, executionOptions?.CommandTimeoutSeconds,
                cancellationToken: cancellationToken);

            using var reader = await connection.ExecuteReaderAsync(command);
            var rowParser = reader.GetRowParser<TSource>();

            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return rowParser(reader);
            }
        }

        public static Task<IEnumerable<TSource>> QueryAsync<TSource>(
            this IDbConnection connection,
            QueryObject queryObject,
            SqlExecutionOptions? executionOptions = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(queryObject.Sql, queryObject.QueryParams, null, executionOptions?.CommandTimeoutSeconds,
                cancellationToken: cancellationToken);
            return connection.QueryAsync<TSource>(command);
        }

        public static Task<int> ExecuteAsync(
            this IDbConnection connection,
            QueryObject queryObject,
            SqlExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(queryObject.Sql, queryObject.QueryParams, null, executionOptions?.CommandTimeoutSeconds,
                cancellationToken: cancellationToken);
            return connection.ExecuteAsync(command);
        }

        public static Task<TSource> ExecuteScalarAsync<TSource>(
            this IDbConnection connection,
            QueryObject queryObject,
            SqlExecutionOptions? executionOptions = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(queryObject.Sql, queryObject.QueryParams, null, executionOptions?.CommandTimeoutSeconds,
                cancellationToken: cancellationToken);
            return connection.ExecuteScalarAsync<TSource>(command);
        }
    }
}