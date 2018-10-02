namespace Byndyusoft.Extensions.Specifications.Linq
{
    using System;
    using System.Linq.Expressions;
    using Impl;

    public static class LinqSpecification
    {
        public static LinqSpecification<T> Empty<T>()
        {
            return new EmptyLinqSpecification<T>();
        }

        public static LinqSpecification<T> True<T>()
        {
            return new TrueLinqSpecification<T>();
        }

        public static LinqSpecification<T> False<T>()
        {
            return new FalseLinqSpecification<T>();
        }

        public static LinqSpecification<T> Create<T>(Expression<Func<T, bool>> expression)
        {
            return new LinqSpecification<T>(expression);
        }
    }
}