namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.Extensions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using SessionsFactory;

    public static class SessionExtensions
    {
        public static IEnumerable<TSource> Query<TSource>(this ISession session, QueryObject queryObject, SqlExecutionOptions executionOptions)
        {
            return session.Query<TSource>(queryObject.Sql, queryObject.QueryParams, commandTimeout: executionOptions.CommandTimeoutSeconds);
        }

        public static Task<IEnumerable<TSource>> QueryAsync<TSource>(this ISession session, QueryObject queryObject, SqlExecutionOptions executionOptions)
        {
            return session.QueryAsync<TSource>(queryObject.Sql, queryObject.QueryParams, executionOptions.CommandTimeoutSeconds);
        }

        public static int Execute(this ISession session, QueryObject queryObject, SqlExecutionOptions executionOptions)
        {
            return session.Execute(queryObject.Sql, queryObject.QueryParams, executionOptions.CommandTimeoutSeconds);
        }

        public static Task<int> ExecuteAsync(this ISession session, QueryObject queryObject, SqlExecutionOptions executionOptions)
        {
            return session.ExecuteAsync(queryObject.Sql, queryObject.QueryParams, executionOptions.CommandTimeoutSeconds);
        }
    }
}