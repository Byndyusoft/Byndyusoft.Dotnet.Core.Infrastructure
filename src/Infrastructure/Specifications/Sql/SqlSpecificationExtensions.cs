namespace Byndyusoft.Extensions.Specifications.Sql
{
    public static class SqlSpecificationExtensions
    {
        public static string Where<T>(this string query, ISqlSpecification<T> specification)
        {
            if (specification == null)
                return query;

            var where = specification.ToSql();
            if (string.IsNullOrWhiteSpace(where))
                return query;

            var sql = $"{query} WHERE {where}";
            return sql;
        }
    }
}