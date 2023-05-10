namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.SessionsFactory
{
    using System;
    using System.Data;
    using ConnectionsFactory;

    public class SessionsFactory : ISessionsFactory
    {
        private readonly IDbConnectionsFactory _dbConnectionsFactory;

        public SessionsFactory(IDbConnectionsFactory dbConnectionsFactory)
        {
            _dbConnectionsFactory = dbConnectionsFactory ?? throw new ArgumentNullException(nameof(dbConnectionsFactory));
        }

        public ISession Create() => Create(IsolationLevel.Unspecified);
        
        public ISession Create(IsolationLevel isolationLevel)
        {
            var connection = _dbConnectionsFactory.Create();
            connection.Open();
            var transaction = connection.BeginTransaction(isolationLevel);
            return new Session(connection, transaction);
        }
    }
}