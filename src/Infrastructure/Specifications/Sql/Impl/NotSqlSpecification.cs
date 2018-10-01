namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    public sealed class NotSqlSpecification<T> : SqlSpecification<T>
    {
        internal NotSqlSpecification(SqlSpecification<T> inner)
            : base(Clause(inner), (object)inner.Parameters)
        {
            Inner = inner;
        }

        public SqlSpecification<T> Inner { get; }

        public override SqlSpecification<T> Not()
        {
            return Inner;
        }

        private static string Clause(SqlSpecification<T> inner)
        {
            var innerSql = inner.ToSql();

            return $"NOT {innerSql}";
        }
    }
}