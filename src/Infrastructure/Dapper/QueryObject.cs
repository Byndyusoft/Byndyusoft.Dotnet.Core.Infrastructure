namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper
{
    using System;
    using global::Dapper;

    /// <summary>
    ///     Incapsulate SQL and Parameters for Dapper methods
    /// </summary>
    /// <remarks>
    ///     http://www.martinfowler.com/eaaCatalog/queryObject.html
    /// </remarks>
    public class QueryObject
    {
        /// <summary>
        ///     Create QueryObject for <paramref name="sql" /> parametrised string
        /// </summary>
        /// <param name="sql">SQL string</param>
        public QueryObject(FormattableString sql)
        {
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));
                
            if (sql.ArgumentCount == 0)
            {
                Sql = sql.Format;
            } 
            else
            {
                var arguments = sql.GetArguments();
                var parameters = new DynamicParameters();
                for (var i = 0; i < arguments.Length; i++)
                    parameters.Add("@p" + i, arguments[i]);
                QueryParams = parameters;
                Sql = string.Format(sql.Format, parameters.ParameterNames);
            }
        }
        
        /// <summary>
        ///     Create QueryObject for parameterized <paramref name="sql" />
        /// </summary>
        /// <param name="sql">SQL string</param>
        /// <param name="queryParams">Parameter list</param>
        public QueryObject(string sql, object queryParams)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException(nameof(sql));
            Sql = sql;
            QueryParams = queryParams;
        }

        /// <summary>
        ///     SQL string
        /// </summary>
        public string Sql { get; }

        /// <summary>
        ///     Parameter list
        /// </summary>
        public object QueryParams { get; }
    }
}
