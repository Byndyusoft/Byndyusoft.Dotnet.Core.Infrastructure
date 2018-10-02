namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    public sealed class NotSqlSpecification : SqlSpecification
    {
        internal NotSqlSpecification(SqlSpecification inner)
            : base(Clause(inner), (object)inner.Parameters)
        {
            Inner = inner;
        }

        public SqlSpecification Inner { get; }

        public override SqlSpecification Not()
        {
            return Inner;
        }

        private static string Clause(SqlSpecification inner)
        {
            var innerSql = inner.ToSql();

            return $"NOT {innerSql}";
        }
    }
}