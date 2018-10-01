namespace Byndyusoft.Extensions.Specifications.Tests
{
    using Complex;
    using Linq;
    using Sql;
    using Xunit;

    public class ComplexSpecificationTests
    {
        [Fact]
        public void Empty_Test()
        {
            var empty = ComplexSpecification.Empty<int>();

            Assert.True(empty.Linq.IsEmpty);
            Assert.True(empty.Sql.IsEmpty);
        }

        [Fact]
        public void False_Test()
        {
            var specification = ComplexSpecification.False<int>();

            Assert.True(specification.Linq.IsFalse);
            Assert.True(specification.Sql.IsFalse);
        }

        [Fact]
        public void True_Test()
        {
            var specification = ComplexSpecification.True<int>();

            Assert.True(specification.Linq.IsTrue);
            Assert.True(specification.Sql.IsTrue);
        }

        [Fact]
        public void Create_Test()
        {
            var linq = LinqSpecification.Create<int>(x => x == 1);
            var sql = SqlSpecification.Create<int>("Id=1");
            var specification = ComplexSpecification.Create(linq, sql);

            Assert.Equal(linq, specification.Linq);
            Assert.Equal(sql, specification.Sql);
        }

        [Fact]
        public void Create_From_Sql_Test()
        {
            var sql = SqlSpecification.Create<int>("Id=1");
            var specification = ComplexSpecification.Create(sql);

            Assert.True(specification.Linq.IsEmpty);
            Assert.Equal(sql, specification.Sql);
        }

        [Fact]
        public void Create_From_Linq_Test()
        {
            var linq = LinqSpecification.Create<int>(x => x == 1);
            var specification = ComplexSpecification.Create(linq);

            Assert.True(specification.Sql.IsEmpty);
            Assert.Equal(linq, specification.Linq);
        }
    }
}