namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    using System.Dynamic;

    public sealed class OrSqlSpecification : SqlSpecification
    {
        internal OrSqlSpecification(SqlSpecification left, SqlSpecification right)
            : base(Clause(left, right), Params(left, right))
        {
            Left = left;
            Right = right;
        }

        public SqlSpecification Left { get; }
        public SqlSpecification Right { get; }

        private static string Clause(SqlSpecification left, SqlSpecification right)
        {
            var leftSql = left.ToSql();
            var rightSql = right.ToSql();

            return $"{leftSql} OR {rightSql}";
        }

        private static ExpandoObject Params(SqlSpecification left, SqlSpecification right)
        {
            return new ExpandoObject { (object)left.Parameters, (object)right.Parameters };
        }
    }
}