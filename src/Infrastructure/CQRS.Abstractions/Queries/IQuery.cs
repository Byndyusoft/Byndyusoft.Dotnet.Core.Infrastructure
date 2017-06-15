namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Queries
{
    /// <summary>
    /// Interface for queries
    /// </summary>
    /// <typeparam name="TCriterion">Query criterion type</typeparam>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IQuery<in TCriterion, out TResult> where TCriterion : ICriterion<TResult>
    {
        /// <summary>
        /// Method for criterion execution
        /// </summary>
        /// <param name="criterion">Information needed for criterion execution</param>
        /// <returns>Query result</returns>
        TResult Ask(TCriterion criterion);
    }
}