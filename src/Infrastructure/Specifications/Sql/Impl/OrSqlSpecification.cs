namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    using System.Dynamic;

    public sealed class OrSqlSpecification<T> : SqlSpecification<T>
    {
        internal OrSqlSpecification(SqlSpecification<T> left, SqlSpecification<T> right)
            : base(Clause(left, right), Params(left, right))
        {
            Left = left;
            Right = right;
        }

        public SqlSpecification<T> Left { get; }
        public SqlSpecification<T> Right { get; }

        private static string Clause(SqlSpecification<T> left, SqlSpecification<T> right)
        {
            var leftSql = left.ToSql();
            var rightSql = right.ToSql();

            return $"{leftSql} OR {rightSql}";
        }

        private static ExpandoObject Params(SqlSpecification<T> left, SqlSpecification<T> right)
        {
            return new ExpandoObject { (object)left.Parameters, (object)right.Parameters };
        }
    }
}