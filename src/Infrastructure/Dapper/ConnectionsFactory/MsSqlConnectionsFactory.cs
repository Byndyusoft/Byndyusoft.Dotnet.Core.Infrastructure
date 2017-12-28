namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.ConnectionsFactory
{
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Options;

    public class MsSqlConnectionsFactory : SqlConnectionsFactoryBase
    {
        public MsSqlConnectionsFactory(IOptions<SqlConnectionsFactoryOptions> options) : base(options)
        {
        }

        protected override IDbConnection NewSqlConnection()
        {
            return new SqlConnection(_options.SqlServer);
        }
    }
}