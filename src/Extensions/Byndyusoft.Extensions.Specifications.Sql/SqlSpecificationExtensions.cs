namespace Byndyusoft.Extensions.Specifications.Sql
{
    public static class SqlSpecificationExtensions
    {
        public static ISqlSpecification<T> AsSql<T>(this ISpecification specification)
        {
            return (ISqlSpecification<T>)specification;
        }

        public static string Where(this string query, ISqlSpecification specification)
        {
            if (specification == null)
                return query;

            var where = specification.ToSql();
            if (string.IsNullOrWhiteSpace(where))
                return query;

            var sql = $"{query} WHERE {where}";
            return sql;
        }

        internal static bool IsSpecialCase(this ISqlSpecification specification)
        {
            return specification.IsEmpty || specification.IsTrue || specification.IsFalse;
        }
    }
}