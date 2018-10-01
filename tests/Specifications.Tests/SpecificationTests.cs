namespace Byndyusoft.Extensions.Specifications.Tests
{
    using Linq;
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
    }
}