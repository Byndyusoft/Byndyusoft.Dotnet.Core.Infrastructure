namespace Byndyusoft.Extensions.Specifications.Sql
{
    using Impl;

    public static class SqlSpecification
    {
        public static SqlSpecification<T> Empty<T>()
        {
            return new EmptySqlSpecification<T>();
        }

        public static SqlSpecification<T> True<T>()
        {
            return new TrueSqlSpecification<T>();
        }

        public static SqlSpecification<T> False<T>()
        {
            return new FalseSqlSpecification<T>();
        }

        public static SqlSpecification<T> Create<T>(string where, object param = null)
        {
            return new SqlSpecification<T>(where, param);
        }
    }
}