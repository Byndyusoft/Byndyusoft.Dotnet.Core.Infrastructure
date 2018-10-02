namespace Byndyusoft.Extensions.Specifications.Complex
{
    using System;
    using Linq;
    using Sql;

    public static class ComplexSpecification
    {
        public static ComplexSpecification<T> Empty<T>()
        {
            var linq = LinqSpecification.Empty<T>();
            var sql = SqlSpecification.Empty();

            return new ComplexSpecification<T>(linq, sql);
        }

        public static ComplexSpecification<T> True<T>()
        {
            var linq = LinqSpecification.True<T>();
            var sql = SqlSpecification.True();

            return new ComplexSpecification<T>(linq, sql);
        }

        public static ComplexSpecification<T> False<T>()
        {
            var linq = LinqSpecification.False<T>();
            var sql = SqlSpecification.False();

            return new ComplexSpecification<T>(linq, sql);
        }

        public static ComplexSpecification<T> Create<T>(LinqSpecification<T> linq, SqlSpecification sql)
        {
            if (linq == null) throw new ArgumentNullException(nameof(linq));
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return new ComplexSpecification<T>(linq, sql);
        }

        public static ComplexSpecification<T> Create<T>(LinqSpecification<T> linq)
        {
            if (linq == null) throw new ArgumentNullException(nameof(linq));

            var sql = SqlSpecification.Empty();
            return Create(linq, sql);
        }

        public static ComplexSpecification<T> Create<T>(SqlSpecification sql)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            var linq = LinqSpecification.Empty<T>();
            return Create(linq, sql);
        }
    }
}