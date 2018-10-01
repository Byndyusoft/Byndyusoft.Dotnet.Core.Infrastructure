namespace Byndyusoft.Extensions.Specifications.Linq
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public static class QueryableAsyncExtensions
    {
        public static async Task<IQueryable<T>> WhereAsync<T, TSource>(this Task<TSource> task, ILinqSpecification<T> specification)
            where TSource : IQueryable<T>
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var items = await task.ConfigureAwait(false);
            return items.Where(specification);
        }

        public static async Task<T> SingleAsync<T, TSource>(this Task<TSource> task, ILinqSpecification<T> specification)
            where TSource : IQueryable<T>
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var items = await task.ConfigureAwait(false);
            return items.Single(specification);
        }

        public static async Task<T> SingleOrDefaultAsync<T, TSource>(this Task<TSource> task, ILinqSpecification<T> specification)
            where TSource : IQueryable<T>
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var items = await task.ConfigureAwait(false);
            return items.SingleOrDefault(specification);
        }

        public static async Task<T> Last<T, TSource>(this Task<TSource> task, ILinqSpecification<T> specification)
            where TSource : IQueryable<T>
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var items = await task.ConfigureAwait(false);
            return items.Last(specification);
        }

        public static async Task<T> LastOrDefaultAsync<T, TSource>(this Task<TSource> task, ILinqSpecification<T> specification)
            where TSource : IQueryable<T>
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var items = await task.ConfigureAwait(false);
            return items.LastOrDefault(specification);
        }

        public static async Task<bool> AllAsync<T, TSource>(this Task<TSource> task, ILinqSpecification<T> specification)
            where TSource : IQueryable<T>
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var items = await task.ConfigureAwait(false);
            return items.All(specification);
        }

        public static async Task<bool> AnyAsync<T, TSource>(this Task<TSource> task, ILinqSpecification<T> specification)
            where TSource : IQueryable<T>
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var items = await task.ConfigureAwait(false);
            return items.Any(specification);
        }

        public static async Task<int> CountAsync<T, TSource>(this Task<TSource> task, ILinqSpecification<T> specification)
            where TSource : IQueryable<T>
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var items = await task.ConfigureAwait(false);
            return items.Count(specification);
        }

        public static async Task<long> LongCountAsync<T, TSource>(this Task<TSource> task, ILinqSpecification<T> specification)
            where TSource : IQueryable<T>
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var items = await task.ConfigureAwait(false);
            return items.LongCount(specification);
        }
    }
}