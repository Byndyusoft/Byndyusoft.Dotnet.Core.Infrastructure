namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Queries
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Queries;

    public class QueriesDispatcher : IQueriesDispatcher
    {
        private readonly IQueriesFactory _queriesFactory;
        private readonly MethodInfo _createQueryGenericDefinition;
        private readonly string _askMethodName;

        public QueriesDispatcher(IQueriesFactory queriesFactory)
        {
            _queriesFactory = queriesFactory ?? throw new ArgumentNullException(nameof(queriesFactory));

            Expression<Func<IQueriesFactory, object>> createQueryExpression =
                x => x.Create<ICriterion<object>, object>();
            _createQueryGenericDefinition =
                ((MethodCallExpression) createQueryExpression.Body).Method.GetGenericMethodDefinition();

            _askMethodName = nameof(IQuery<ICriterion<object>, object>.Ask);
        }

        public Task<TResult> Execute<TResult>(ICriterion<TResult> criterion, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var createMethod = _createQueryGenericDefinition.MakeGenericMethod(criterion.GetType(), typeof(TResult));

            var query = createMethod.Invoke(_queriesFactory, null)!;
            var ackMethod = query.GetType().GetRuntimeMethod(
                _askMethodName,
                new[]
                {
                    criterion.GetType(),
                    typeof(CancellationToken)
                })!;
            try
            {
                return (Task<TResult>) ackMethod.Invoke(
                    query,
                    new object[]
                    {
                        criterion,
                        cancellationToken
                    })!;
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
            }

            return default!;
        }

        public Task<TResult> Execute<TResult>(ICriterion<TResult> criterion)
        {
            return Execute(criterion, CancellationToken.None);
        }
    }
}