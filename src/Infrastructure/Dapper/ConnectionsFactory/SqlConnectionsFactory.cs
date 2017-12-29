namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.ConnectionsFactory
{
    using System;
    using System.Data;
    using Microsoft.Extensions.Options;

    public class SqlConnectionsFactory<T> : IDbConnectionsFactory where T : IDbConnection
    {
        protected readonly SqlConnectionsFactoryOptions _options;

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
            var sqlConnection = NewSqlConnection();
            sqlConnection.Open();
            return sqlConnection;
        }

        protected virtual IDbConnection NewSqlConnection()
        {
            return (T) Activator.CreateInstance(typeof(T), _options.SqlServer);
        }
    }
}