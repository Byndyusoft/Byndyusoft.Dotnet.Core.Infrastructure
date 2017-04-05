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
            if (dbConnectionsFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionsFactory));

            _dbConnectionsFactory = dbConnectionsFactory;
        }

        public ISession Create(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var connection = _dbConnectionsFactory.Create();
            var transaction = connection.BeginTransaction(isolationLevel);

            return new Session(connection, transaction);
        }
    }
}