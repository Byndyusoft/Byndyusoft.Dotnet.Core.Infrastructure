namespace Byndyusoft.Dotnet.Core.Samples.Web.DataAccess.Values.Queries
{
    using System;
    using Domain.Criterions.Values;
    using Infrastructure.CQRS.Abstractions.Queries;
    using JetBrains.Annotations;
    using Repository;

    [UsedImplicitly]
    public class GetValueQuery : IQuery<GetValueQueryCriterion, string>
    {
        private readonly IValuesRepository _repository;

        public GetValueQuery(IValuesRepository repository)
        {
            if(repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        public string Ask(GetValueQueryCriterion criterion)
        {
            return _repository.Get(criterion.Id);
        }
    }
}