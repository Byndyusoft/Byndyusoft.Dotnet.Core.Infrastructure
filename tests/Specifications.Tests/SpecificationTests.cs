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
            var specification = SqlSpecification.Create<int>("Id=1");

            var linq = specification.AsSql();

            Assert.IsAssignableFrom<ISqlSpecification<int>>(linq);
        }
    }
}