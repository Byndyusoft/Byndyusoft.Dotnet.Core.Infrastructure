namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.ConnectionsFactory
{
    using System;
    using System.Data.Common;

    public class DbConnectionsFactory : IDbConnectionsFactory
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly string _connectionString;

        public DbConnectionsFactory(DbProviderFactory dbProviderFactory, string connectionString)
        {
            _dbProviderFactory = dbProviderFactory ?? throw new ArgumentNullException(nameof(dbProviderFactory));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        
        public DbConnection Create()
        {
            var connection = _dbProviderFactory.CreateConnection()!;
            connection.ConnectionString = _connectionString;
            return connection;
        }
    }
}