namespace Byndyusoft.Extensions.Specifications.Sql
{
    using Impl;

    public partial class SqlSpecification
    {
        public static SqlSpecification Empty()
        {
            return new EmptySqlSpecification();
        }

        public static SqlSpecification<T> Empty<T>()
        {
            return new EmptySqlSpecification();
        }

        public static SqlSpecification True()
        {
            return new TrueSqlSpecification();
        }

        public static SqlSpecification<T> True<T>()
        {
            return new TrueSqlSpecification();
        }

        public static SqlSpecification False()
        {
            return new FalseSqlSpecification();
        }

        public static SqlSpecification<T> False<T>()
        {
            return new FalseSqlSpecification();
        }

        public static SqlSpecification Create(string where, object param = null)
        {
            return new SqlSpecification(where, param);
        }

        public static SqlSpecification<T> Create<T>(string where, object param = null)
        {
            return new SqlSpecification(where, param);
        }
    }
}