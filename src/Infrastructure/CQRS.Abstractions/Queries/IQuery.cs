namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Queries
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for queries
    /// </summary>
    /// <typeparam name="TCriterion">Query criterion type</typeparam>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IQuery<in TCriterion, TResult> where TCriterion : ICriterion<TResult>
    {
        /// <summary>
        /// Method for criterion execution
        /// </summary>
        /// <param name="criterion">Information needed for criterion execution</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Query result</returns>
        Task<TResult> Ask(TCriterion criterion, CancellationToken cancellationToken);
    }
}