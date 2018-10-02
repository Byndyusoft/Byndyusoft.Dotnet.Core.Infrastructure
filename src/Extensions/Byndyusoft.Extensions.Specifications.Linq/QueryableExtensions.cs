namespace Byndyusoft.Extensions.Specifications.Linq
{
    using System;
    using System.Linq;

    public static class QueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Where(specification.Expression);
        }

        public static T Single<T>(this IQueryable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Single(specification.Expression);
        }

        public static T SingleOrDefault<T>(this IQueryable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.SingleOrDefault(specification.Expression);
        }

        public static T Last<T>(this IQueryable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Last(specification.Expression);
        }

        public static T LastOrDefault<T>(this IQueryable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.LastOrDefault(specification.Expression);
        }

        public static bool All<T>(this IQueryable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.All(specification.Expression);
        }

        public static bool Any<T>(this IQueryable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Any(specification.Expression);
        }

        public static int Count<T>(this IQueryable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Count(specification.Expression);
        }

        public static long LongCount<T>(this IQueryable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.LongCount(specification.Expression);
        }
    }
}