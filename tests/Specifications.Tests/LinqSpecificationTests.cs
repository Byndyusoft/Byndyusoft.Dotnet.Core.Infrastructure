namespace Byndyusoft.Extensions.Specifications.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Linq;
    using Xunit;

    public class LinqSpecificationTests
    {
        private T[] Where<T>(IEnumerable<T> source, LinqSpecification<T> specification)
        {
            return source.Where(specification.Predicate).ToArray();
        }

        [Fact]
        public void LinqSpecification_Test()
        {
            var array = new[] {1, 2, 3};
            var specification = LinqSpecification.Create<int>(x => x == 3);

            var result = Where(array, specification);

            Assert.Equal(new[] {3}, result);
        }

        [Fact]
        public void EmptySpecification_Test()
        {
            var array = new[] {1, 2, 3};
            var specification = LinqSpecification.Empty<int>();

            var result = Where(array, specification);

            Assert.Equal(array, result);
            Assert.True(specification.IsEmpty);
        }

        [Fact]
        public void EmptySpecification_And_Test()
        {
            var empty = LinqSpecification.Empty<int>();
            var other = LinqSpecification.Create<int>(x => x == 1);

            var specification = empty.And(other);

            Assert.Equal(other, specification);
        }

        [Fact]
        public void EmptySpecification_Or_Test()
        {
            var empty = LinqSpecification.Empty<int>();
            var other = LinqSpecification.Create<int>(x => x == 1);

            var specification = empty.Or(other);

            Assert.Equal(other, specification);
        }

        [Fact]
        public void EmptySpecification_Not_Test()
        {
            var empty = LinqSpecification.Empty<int>();

            var specification = empty.Not();

            Assert.Equal(empty, specification);
        }

        [Fact]
        public void TrueSpecification_Test()
        {
            var array = new[] {1, 2, 3};
            var specification = LinqSpecification.True<int>();

            var result = Where(array, specification);

            Assert.Equal(array, result);
            Assert.True(specification.IsTrue);
        }

        [Fact]
        public void TrueSpecification_And_Test()
        {
            var tru = LinqSpecification.True<int>();
            var other = LinqSpecification.Create<int>(x => x == 1);

            var specification = tru.And(other);

            Assert.Equal(other, specification);
        }

        [Fact]
        public void TrueSpecification_Or_Test()
        {
            var tru = LinqSpecification.True<int>();
            var other = LinqSpecification.Create<int>(x => x == 1);

            var specification = tru.Or(other);

            Assert.Equal(tru, specification);
        }

        [Fact]
        public void TrueSpecification_Not_Test()
        {
            var tru = LinqSpecification.True<int>();

            var specification = tru.Not();

            Assert.True(specification.IsFalse);
        }

        [Fact]
        public void FalseSpecification_Test()
        {
            var array = new[] {1, 2, 3};
            var specification = LinqSpecification.False<int>();

            var result = Where(array, specification);

            Assert.Empty(result);
            Assert.True(specification.IsFalse);
        }

        [Fact]
        public void FalseSpecification_And_Test()
        {
            var flse = LinqSpecification.False<int>();
            var other = LinqSpecification.Create<int>(x => x == 1);

            var specification = flse.And(other);

            Assert.Equal(flse, specification);
        }

        [Fact]
        public void FalseSpecification_Or_Test()
        {
            var flse = LinqSpecification.False<int>();
            var other = LinqSpecification.Create<int>(x => x == 1);

            var specification = flse.Or(other);

            Assert.Equal(other, specification);
        }

        [Fact]
        public void FalseSpecification_Not_Test()
        {
            var flse = LinqSpecification.False<int>();

            var specification = flse.Not();

            Assert.True(specification.IsTrue);
        }

        [Fact]
        public void AndSpecification_Test()
        {
            var array = new[] {1, 2, 3};
            var specification1 = LinqSpecification.Create<int>(x => x == 2);
            var specification2 = LinqSpecification.Create<int>(x => x % 2 == 0);
            var specification = specification1.And(specification2);

            var result = Where(array, specification);

            Assert.Equal(new[] {2}, result);
        }

        [Fact]
        public void OrSpecification_Test()
        {
            var array = new[] {1, 2, 3};
            var specification1 = LinqSpecification.Create<int>(x => x == 2);
            var specification2 = LinqSpecification.Create<int>(x => x ==3);
            var specification = specification1.Or(specification2);

            var result = Where(array, specification);

            Assert.Equal(new[] {2, 3}, result);
        }

        [Fact]
        public void NotSpecification_Test()
        {
            var array = new[] {1, 2, 3};
            var specification = LinqSpecification.Create<int>(x => x == 2).Not();

            var result = Where(array, specification);

            Assert.Equal(new[] {1, 3}, result);
        }

        [Fact]
        public void ComposeSpecification_Test()
        {
            var array = new[] {1, 2, 3};
            var specification1 = LinqSpecification.Create<int>(x => x == 2)
                .And(LinqSpecification.Create<int>(x => x % 2 == 0));
            var specification2 = LinqSpecification.Create<int>(x => x == 3);
            var specification = specification1.Or(specification2);

            var result = Where(array, specification);

            Assert.Equal(new[] {2, 3}, result);
        }

        [Fact]
        public void TrueSpecification_Negative_Test()
        {
            var array = new[] {1, 2, 3};
            var specification = LinqSpecification.True<int>().Not();

            var result = Where(array, specification);

            Assert.Empty(result);
        }

        [Fact]
        public void FalseSpecification_Negative_Test()
        {
            var array = new[] {1, 2, 3};
            var specification = LinqSpecification.False<int>().Not();

            var result = Where(array, specification);

            Assert.Equal(array, result);
        }

        [Fact]
        public void NotSpecification_Negative_Test()
        {
            var array = new[] {1, 2, 3};
            var specification = LinqSpecification.Create<int>(x => x == 2).Not().Not();

            var result = Where(array, specification);

            Assert.Equal(new[] {2}, result);
        }

        [Fact]
        public void AndOperator_Test()
        {
            var array = new[] {1, 2, 3};
            var specification1 = LinqSpecification.Create<int>(x => x == 2);
            var specification2 = LinqSpecification.Create<int>(x => x % 2 == 0);
            var specification = specification1 & specification2;

            var result = Where(array, specification);

            Assert.Equal(new[] {2}, result);
        }

        [Fact]
        public void OrOperator_Test()
        {
            var array = new[] {1, 2, 3};
            var specification1 = LinqSpecification.Create<int>(x => x == 2);
            var specification2 = LinqSpecification.Create<int>(x => x == 3);
            var specification = specification1 | specification2;

            var result = Where(array, specification);

            Assert.Equal(new[] {2, 3}, result);
        }

        [Fact]
        public void NotOperator_Test()
        {
            var array = new[] {1, 2, 3};
            var specification1 = LinqSpecification.Create<int>(x => x == 2);
            var specification = !specification1;

            var result = Where(array, specification);

            Assert.Equal(new[] {1, 3}, result);
        }
    }
}