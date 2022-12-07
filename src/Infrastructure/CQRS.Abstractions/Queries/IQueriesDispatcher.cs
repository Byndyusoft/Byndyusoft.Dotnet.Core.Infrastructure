namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Queries
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Queries dispatcher interface
    /// </summary>
    public interface IQueriesDispatcher
    {
        /// <summary>
        /// Method for queries execution
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="criterion">Information needed for queries execution</param>
        /// <returns>Query result</returns>
        Task<TResult> Execute<TResult>(ICriterion<TResult> criterion);

        /// <summary>
        /// Method for queries execution
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="criterion">Information needed for queries execution</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Query result</returns>
        Task<TResult> Execute<TResult>(ICriterion<TResult> criterion, CancellationToken cancellationToken);
    }
}