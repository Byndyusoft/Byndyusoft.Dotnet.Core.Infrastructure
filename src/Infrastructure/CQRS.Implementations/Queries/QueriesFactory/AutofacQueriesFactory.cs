namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Queries.QueriesFactory
{
    using System;
    using Abstractions.Queries;
    using Autofac;

    /// <summary>
    /// Queries factory for Autofac
    /// </summary>
    public class AutofacQueriesFactory : IQueriesFactory
    {
        private readonly IComponentContext _componentContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentContext">Autofac components context</param>
        public AutofacQueriesFactory(IComponentContext componentContext)
        {
            if (componentContext == null)
                throw new ArgumentNullException(nameof(componentContext));

            _componentContext = componentContext;
        }

        /// <summary>
        /// Method for qsueries creation
        /// </summary>
        /// <typeparam name="TCriterion">Query criterion type</typeparam>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <returns>Query instance</returns>
        public IQuery<TCriterion, TResult> Create<TCriterion, TResult>() where TCriterion : ICriterion
        {
            return _componentContext.Resolve<IQuery<TCriterion, TResult>>();
        }
    }
}