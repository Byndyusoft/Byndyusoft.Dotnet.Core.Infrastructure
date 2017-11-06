namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper
{
    using System;
    using System.Linq;
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
        public QueryObject(FormattableString sql) : this(sql, new DynamicParameters())
        {
        }
        
        /// <summary>
        ///     Create QueryObject for <paramref name="sql" /> parametrised string
        /// </summary>
        /// <param name="sql">SQL string</param>
        public QueryObject(StringIfNotFormattableStringAdapter sql)
        {
            if (string.IsNullOrEmpty(sql?.String))
                throw new ArgumentNullException("sql");

            Sql = sql.String;
        }

        /// <summary>
        ///     Create QueryObject for parameterized <paramref name="sql" />
        /// </summary>
        /// <param name="sql">SQL string</param>
        /// <param name="queryParams">Parameter list</param>
        public QueryObject(StringIfNotFormattableStringAdapter sql, object queryParams) : this(sql)
        {
            QueryParams = queryParams;
        }

        private QueryObject(FormattableString sql, DynamicParameters dynamicParameters)
        {
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));

            if (sql.ArgumentCount == 0)
            {
                Sql = sql.Format;
                return;
            }

            var arguments = sql.GetArguments();
            for (var i = 0; i < arguments.Length; i++)
                dynamicParameters.Add("p" + i, arguments[i]);
            QueryParams = dynamicParameters;
            Sql = string.Format(sql.Format, dynamicParameters.ParameterNames.Select(p => "@" + p).Cast<object>().ToArray());
        }

        /// <summary>
        ///     Create QueryObject for parameterized <paramref name="sql" />
        /// </summary>
        /// <param name="sql">SQL string</param>
        /// <param name="queryParams">Parameter list</param>
        public QueryObject(FormattableString sql, object queryParams) : this(sql, new DynamicParameters(queryParams))
        {
        }

        /// <summary>
        ///     SQL string
        /// </summary>
        public string Sql { get; }

        /// <summary>
        ///     Parameter list
        /// </summary>
        public object QueryParams { get;}
    }

    public class StringIfNotFormattableStringAdapter
    {
        public string String { get; }

        public StringIfNotFormattableStringAdapter(string s)
        {
            String = s;
        }

        public static implicit operator StringIfNotFormattableStringAdapter(string s)
        {
            return new StringIfNotFormattableStringAdapter(s);
        }

        public static implicit operator StringIfNotFormattableStringAdapter(FormattableString fs)
        {
            throw new InvalidOperationException(
                "Missing FormattableString overload of method taking this type as argument");
        }
    }
}
