namespace Byndyusoft.Extensions.Specifications.Linq
{
    using System;
    using System.Linq.Expressions;

    public interface ILinqSpecification<T>
    {
        Expression<Func<T, bool>> Expression { get; }

        Func<T, bool> Predicate { get; }
    }
}