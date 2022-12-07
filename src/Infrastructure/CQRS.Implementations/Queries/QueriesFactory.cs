namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Queries
{
    using System;
    using Abstractions.Queries;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Default queries factory
    /// </summary>
    public class QueriesFactory : IQueriesFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">Service provider instance</param>
        public QueriesFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Method for queries creation
        /// </summary>
        /// <typeparam name="TCriterion">Query criterion type</typeparam>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <returns>Query instance</returns>
        public IQuery<TCriterion, TResult> Create<TCriterion, TResult>() where TCriterion : ICriterion<TResult>
        {
            return _serviceProvider.GetRequiredService<IQuery<TCriterion, TResult>>();
        }
    }
}