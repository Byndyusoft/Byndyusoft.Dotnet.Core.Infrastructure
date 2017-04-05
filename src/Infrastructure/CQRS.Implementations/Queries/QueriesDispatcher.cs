namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Queries
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using Abstractions.Queries;
    using QueriesFactory;

    /// <summary>
    /// Queries dispatcher standart implementation
    /// </summary>
    public class QueriesDispatcher : IQueriesDispatcher
    {
        private readonly IQueriesFactory _queriesFactory;

        private readonly MethodInfo _createQueryGenericDefinition;
        private readonly string _askMethodName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queriesFactory">Queries factory</param>
        public QueriesDispatcher(IQueriesFactory queriesFactory)
        {
            if (queriesFactory == null)
                throw new ArgumentNullException(nameof(queriesFactory));

            _queriesFactory = queriesFactory;

            Expression<Func<IQueriesFactory, IQuery<ICriterion, object>>> fakeCreateCall = x => x.Create<ICriterion, object>();
            _createQueryGenericDefinition = ((MethodCallExpression)fakeCreateCall.Body).Method.GetGenericMethodDefinition();

            Expression<Func<IQuery<ICriterion, object>, object>> fakeAskCall = x => x.Ask(null);
            _askMethodName = ((MethodCallExpression)fakeAskCall.Body).Method.Name;
        }

        /// <summary>
        /// Method for queries execution
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="criterion">Information needed for queries execution</param>
        /// <returns>Query result</returns>
        public TResult Execute<TResult>(ICriterion criterion)
        {
            var query = _createQueryGenericDefinition.MakeGenericMethod(criterion.GetType(), typeof(TResult)).Invoke(_queriesFactory, null);
            var askMethodDefinition = query.GetType().GetRuntimeMethod(_askMethodName, new[] { criterion.GetType() });

            try
            {
                return (TResult)askMethodDefinition.Invoke(query, new object[] { criterion });
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            return default(TResult);
        }
    }
}