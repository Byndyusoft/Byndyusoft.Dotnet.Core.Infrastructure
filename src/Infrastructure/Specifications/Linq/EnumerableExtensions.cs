namespace Byndyusoft.Extensions.Specifications.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Where(specification.Predicate);
        }

        public static T Single<T>(this IEnumerable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Single(specification.Predicate);
        }

        public static T SingleOrDefault<T>(this IEnumerable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.SingleOrDefault(specification.Predicate);
        }

        public static T Last<T>(this IEnumerable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Last(specification.Predicate);
        }

        public static T LastOrDefault<T>(this IEnumerable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.LastOrDefault(specification.Predicate);
        }

        public static bool All<T>(this IEnumerable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.All(specification.Predicate);
        }

        public static bool Any<T>(this IEnumerable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Any(specification.Predicate);
        }

        public static int Count<T>(this IEnumerable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Count(specification.Predicate);
        }

        public static long LongCount<T>(this IEnumerable<T> source, ILinqSpecification<T> specification)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.LongCount(specification.Predicate);
        }
    }
}