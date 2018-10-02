namespace Byndyusoft.Extensions.Specifications.Linq.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    internal static class ExpressionExtensions
    {
        public static Expression<Func<T, TResult>> AndAlso<T, TResult>(
            this Expression<Func<T, TResult>> first, Expression<Func<T, TResult>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, TResult>> OrElse<T, TResult>(
            this Expression<Func<T, TResult>> first, Expression<Func<T, TResult>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<Func<T, TResult>> Not<T, TResult>(
            this Expression<Func<T, TResult>> first)
        {
            return Expression.Lambda<Func<T, TResult>>(Expression.Not(first.Body), first.Parameters[0]);
        }

        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
    }
}