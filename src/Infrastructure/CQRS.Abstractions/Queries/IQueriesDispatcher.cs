namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Queries
{
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
        TResult Execute<TResult>(ICriterion<TResult> criterion);
    }
}