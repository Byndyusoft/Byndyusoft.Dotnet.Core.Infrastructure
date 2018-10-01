namespace Byndyusoft.Extensions.Specifications.Complex
{
    using Linq;
    using Sql;

    public static class ComplexSpecification
    {
        public static ComplexSpecification<T> Empty<T>()
        {
            var linq = LinqSpecification.Empty<T>();
            var sql = SqlSpecification.Empty<T>();

            return new ComplexSpecification<T>(linq, sql);
        }

        public static ComplexSpecification<T> True<T>()
        {
            var linq = LinqSpecification.True<T>();
            var sql = SqlSpecification.True<T>();

            return new ComplexSpecification<T>(linq, sql);
        }

        public static ComplexSpecification<T> False<T>()
        {
            var linq = LinqSpecification.False<T>();
            var sql = SqlSpecification.False<T>();

            return new ComplexSpecification<T>(linq, sql);
        }

        public static ComplexSpecification<T> Create<T>(LinqSpecification<T> linq, SqlSpecification<T> sql)
        {
            return new ComplexSpecification<T>(linq, sql);
        }

        public static ComplexSpecification<T> Create<T>(LinqSpecification<T> linq)
        {
            var sql = SqlSpecification.Empty<T>();
            return Create(linq, sql);
        }

        public static ComplexSpecification<T> Create<T>(SqlSpecification<T> sql)
        {
            var linq = LinqSpecification.Empty<T>();
            return Create(linq, sql);
        }
    }
}