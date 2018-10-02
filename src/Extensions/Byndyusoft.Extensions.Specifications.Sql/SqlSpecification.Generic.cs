namespace Byndyusoft.Extensions.Specifications.Sql
{
    public class SqlSpecification<T> : ISqlSpecification<T>
    {
        private readonly SqlSpecification _inner;

        public SqlSpecification(SqlSpecification inner)
        {
            _inner = inner;
        }

        public bool IsEmpty => _inner.IsEmpty;
        public bool IsTrue => _inner.IsTrue;
        public bool IsFalse => _inner.IsFalse;
        public dynamic Parameters => _inner.Parameters;
        public string ToSql() => _inner.ToSql();

        public static implicit operator SqlSpecification<T>(SqlSpecification specification)
        {
            return new SqlSpecification<T>(specification);
        }
    }
}