using Byndyusoft.Dotnet.Core.Infrastructure.Dapper;
using Xunit;

namespace Dapper.Tests
{
    public class QueryObjectTests
    {
        [Fact]
        public void QueryTest()
        {
            var queryObject = new QueryObject(@"
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES(1000)");

            Assert.Equal(@"
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES(1000)", queryObject.Sql);
            Assert.Null(queryObject.QueryParams);
        }

        [Fact]
        public void ParameterizedQueryTest()
        {
            var queryObject = new QueryObject(@"
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES(@version)", new{version = 1000});

            Assert.Equal(@"
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES(@version)", queryObject.Sql);
            Assert.Equal(1000, ((dynamic)queryObject.QueryParams).version);
        }

        [Fact]
        public void FormattedParameterizedStringTest()
        {
            var version = "sql_injection";

            var queryObject = new QueryObject($@"
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES({version});
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES(@version)", new { version = 1000 });

            Assert.Equal(@"
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES(@p0);
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES(@version)", queryObject.Sql);
            Assert.Equal("sql_injection", ((DynamicParameters)queryObject.QueryParams).Get<string>("p0"));
        }

        [Fact]
        public void FormattedStringTest()
        {
            var version = "sql_injection";

            var queryObject = new QueryObject($@"
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES({version})");

            Assert.Equal(@"
            INSERT INTO ""VersionInfo""(""Version"")
            VALUES(@p0)", queryObject.Sql);
            Assert.Equal("sql_injection", ((DynamicParameters)queryObject.QueryParams).Get<string>("p0"));
        }
    }
}
