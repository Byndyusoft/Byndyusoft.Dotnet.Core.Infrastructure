namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.ConnectionsFactory
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Options;

    public class SqlConnectionsFactory : IDbConnectionsFactory
    {
        private readonly SqlConnectionsFactoryOptions _options;

        public SqlConnectionsFactory(IOptions<SqlConnectionsFactoryOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(options.Value.SqlServer))
                throw new ArgumentNullException(nameof(SqlConnectionsFactoryOptions.SqlServer));

            _options = options.Value;
        }

        public IDbConnection Create()
        {
            var sqlConnection = new SqlConnection(_options.SqlServer);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}