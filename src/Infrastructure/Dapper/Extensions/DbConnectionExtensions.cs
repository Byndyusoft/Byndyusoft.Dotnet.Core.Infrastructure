namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.Extensions
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using global::Dapper;

    public static class DbConnectionExtensions
    {
        public static IEnumerable<TSource> Query<TSource>(this IDbConnection connection, QueryObject queryObject, SqlExecutionOptions executionOptions = null)
        {
            return connection.Query<TSource>(queryObject.Sql, queryObject.QueryParams, commandTimeout: executionOptions?.CommandTimeoutSeconds);
        }

        public static Task<IEnumerable<TSource>> QueryAsync<TSource>(this IDbConnection connection, QueryObject queryObject, SqlExecutionOptions executionOptions = null)
        {
            return connection.QueryAsync<TSource>(queryObject.Sql, queryObject.QueryParams, commandTimeout: executionOptions?.CommandTimeoutSeconds);
        }

        public static int Execute(this IDbConnection connection, QueryObject queryObject, SqlExecutionOptions executionOptions = null)
        {
            return connection.Execute(queryObject.Sql, queryObject.QueryParams, commandTimeout: executionOptions?.CommandTimeoutSeconds);
        }

        public static Task<int> ExecuteAsync(this IDbConnection connection, QueryObject queryObject, SqlExecutionOptions executionOptions = null)
        {
            return connection.ExecuteAsync(queryObject.Sql, queryObject.QueryParams, commandTimeout: executionOptions?.CommandTimeoutSeconds);
        }
    }
}