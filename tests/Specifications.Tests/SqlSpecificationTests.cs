namespace Byndyusoft.Extensions.Specifications.Tests
{
    using Sql;
    using Xunit;

    public class SqlSpecificationTests
    {
        private string CreateSql<T>(string query, SqlSpecification<T> specification)
        {
            return query.Where(specification);
        }

        [Fact]
        public void Where_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.Create<int>("Id=1");

            var sql = query.Where(specification);

            Assert.Equal("SELECT * FROM Table WHERE (Id=1)", sql);
        }

        [Fact]
        public void Where_Null_Specification_Test()
        {
            const string query = "SELECT * FROM Table";

            var sql = query.Where((ISqlSpecification<int>)null);

            Assert.Equal("SELECT * FROM Table", sql);
        }

        [Fact]
        public void SqlSpecification_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.Create<int>("Id=1");

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE (Id=1)", sql);
        }

        [Fact]
        public void SqlSpecification_Parameters_Test()
        {
            var specification = SqlSpecification.Create<int>("Id=@id", new {id = 1});

            Assert.Equal("Id=@id", specification.Sql);
            Assert.Equal(1, specification.Parameters.id);
        }

        [Fact]
        public void EmptySpecification_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.Empty<int>();

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table", sql);
            Assert.True(specification.IsEmpty);
        }

        [Fact]
        public void EmptySpecification_And_Test()
        {
            var empty = SqlSpecification.Empty<int>();
            var other = SqlSpecification.Create<int>("id=1");

            var specification = empty.And(other);

            Assert.Equal(other, specification);
        }

        [Fact]
        public void EmptySpecification_Or_Test()
        {
            var empty = SqlSpecification.Empty<int>();
            var other = SqlSpecification.Create<int>("id=1");

            var specification = empty.Or(other);

            Assert.Equal(other, specification);
        }

        [Fact]
        public void EmptySpecification_Not_Test()
        {
            var empty = SqlSpecification.Empty<int>();

            var specification = empty.Not();

            Assert.Equal(empty, specification);
        }

        [Fact]
        public void TrueSpecification_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.True<int>();

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table", sql);
            Assert.True(specification.IsTrue);
        }

        [Fact]
        public void TrueSpecification_And_Test()
        {
            var tru = SqlSpecification.True<int>();
            var other = SqlSpecification.Create<int>("id=1");

            var specification = tru.And(other);

            Assert.Equal(other, specification);
        }

        [Fact]
        public void TrueSpecification_Or_Test()
        {
            var tru = SqlSpecification.True<int>();
            var other = SqlSpecification.Create<int>("id=1");

            var specification = tru.Or(other);

            Assert.Equal(tru, specification);
        }

        [Fact]
        public void TrueSpecification_Not_Test()
        {
            var tru = SqlSpecification.True<int>();

            var specification = tru.Not();

            Assert.True(specification.IsFalse);
        }

        [Fact]
        public void FalseSpecification_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.False<int>();

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE (1<>1)", sql);
            Assert.True(specification.IsFalse);
        }

        [Fact]
        public void FalseSpecification_And_Test()
        {
            var flse = SqlSpecification.False<int>();
            var other = SqlSpecification.Create<int>("id=1");

            var specification = flse.And(other);

            Assert.Equal(flse, specification);
        }

        [Fact]
        public void FalseSpecification_Or_Test()
        {
            var flse = SqlSpecification.False<int>();
            var other = SqlSpecification.Create<int>("id=1");

            var specification = flse.Or(other);

            Assert.Equal(other, specification);
        }

        [Fact]
        public void FalseSpecification_Not_Test()
        {
            var flse = SqlSpecification.False<int>();

            var specification = flse.Not();

            Assert.True(specification.IsTrue);
        }

        [Fact]
        public void AndSpecification_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.Create<int>("Id=1").And(SqlSpecification.Create<int>("Id=2"));

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE ((Id=1) AND (Id=2))", sql);
        }

        [Fact]
        public void AndSpecification_Parameters_Test()
        {
            var specification1 = SqlSpecification.Create<int>("Id=@id1", new { id1 = 1 });
            var specification2 = SqlSpecification.Create<int>("Id=@id2", new { id2 = 2 });
            var specification = specification1.And(specification2);

            Assert.Equal(1, specification.Parameters.id1);
            Assert.Equal(2, specification.Parameters.id2);
        }

        [Fact]
        public void OrSpecification_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.Create<int>("Id=1").Or(SqlSpecification.Create<int>("Id=2"));

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE ((Id=1) OR (Id=2))", sql);
        }

        [Fact]
        public void OrSpecification_Parameters_Test()
        {
            var specification1 = SqlSpecification.Create<int>("Id=@id1", new { id1 = 1 });
            var specification2 = SqlSpecification.Create<int>("Id=@id2", new { id2 = 2 });
            var specification = specification1.Or(specification2);

            Assert.Equal(1, specification.Parameters.id1);
            Assert.Equal(2, specification.Parameters.id2);
        }

        [Fact]
        public void NotSpecification_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.Create<int>("Id=1").Not();

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE (NOT (Id=1))", sql);
        }

        [Fact]
        public void ComposeSpecification_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification1 = SqlSpecification.Create<int>("Id=1").Not();
            var specification2 = SqlSpecification.Create<int>("Id=1").And(SqlSpecification.Create<int>("Id=2"));
            var specification = specification1.Or(specification2);

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE ((NOT (Id=1)) OR ((Id=1) AND (Id=2)))", sql);
        }

        [Fact]
        public void TrueSpecification_Negative_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.True<int>().Not();

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE (1<>1)", sql);
        }

        [Fact]
        public void FalseSpecification_Negative_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.False<int>().Not();

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table", sql);
        }

        [Fact]
        public void NotSpecification_Negative_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification = SqlSpecification.Create<int>("Id=1").Not().Not();

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE (Id=1)", sql);
        }

        [Fact]
        public void AndOperator_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification1 = SqlSpecification.Create<int>("Id=1");
            var specification2 = SqlSpecification.Create<int>("Id=2");
            var specification = specification1 & specification2;

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE ((Id=1) AND (Id=2))", sql);
        }

        [Fact]
        public void OrOperator_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification1 = SqlSpecification.Create<int>("Id=1");
            var specification2 = SqlSpecification.Create<int>("Id=2");
            var specification = specification1 | specification2;

            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE ((Id=1) OR (Id=2))", sql);
        }

        [Fact]
        public void NotOperator_Test()
        {
            const string query = "SELECT * FROM Table";
            var specification1 = SqlSpecification.Create<int>("Id=1");
            var specification = !specification1;
            var sql = CreateSql(query, specification);

            Assert.Equal("SELECT * FROM Table WHERE (NOT (Id=1))", sql);
        }
    }
}
