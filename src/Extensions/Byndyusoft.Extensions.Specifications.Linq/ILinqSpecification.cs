namespace Byndyusoft.Extensions.Specifications.Linq
{
    using System;
    using System.Linq.Expressions;

    public interface ILinqSpecification<T> : ISpecification<T>
    {
        Expression<Func<T, bool>> Expression { get; }

        Func<T, bool> Predicate { get; }
    }
}