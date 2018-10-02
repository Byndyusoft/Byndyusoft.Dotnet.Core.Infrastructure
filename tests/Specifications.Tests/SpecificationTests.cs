namespace Byndyusoft.Extensions.Specifications.Tests
{
    using Linq;
    using Sql;
    using Xunit;

    public class SpecificationTests
    {
        [Fact]
        public void AsLinq_Test()
        {
            var specification = LinqSpecification.Create<int>(x => x == 0);

            var linq = specification.AsLinq();

            Assert.IsAssignableFrom<ILinqSpecification<int>>(linq);
        }

        [Fact]
        public void AsSql_Test()
        {
            SqlSpecification<int> specification = SqlSpecification.Create("Id=1");

            var linq = specification.AsSql();

            Assert.IsAssignableFrom<ISqlSpecification<int>>(linq);
        }
    }
}