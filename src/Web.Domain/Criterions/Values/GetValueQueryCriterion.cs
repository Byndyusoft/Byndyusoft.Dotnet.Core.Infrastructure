namespace Byndyusoft.Dotnet.Core.Samples.Web.Domain.Criterions.Values
{
    using Infrastructure.CQRS.Abstractions.Queries;

    public class GetValueQueryCriterion : ICriterion
    {
        public int Id { get; }

        public GetValueQueryCriterion(int id)
        {
            Id = id;
        }
    }
}