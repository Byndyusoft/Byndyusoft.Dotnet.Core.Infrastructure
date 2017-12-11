namespace Byndyusoft.Dotnet.Core.Samples.Web.DataAccess.Db.Queries
{
    using System;
    using Infrastructure.Dapper;
    using Infrastructure.Dapper.ConnectionsFactory;
    using Microsoft.Extensions.Options;

    public abstract class BaseDbQuery
    {
        protected readonly IDbConnectionsFactory ConnectionsFactory;
        protected readonly SqlExecutionOptions SqlExecutionOptions;

        protected BaseDbQuery(IDbConnectionsFactory connectionsFactory, IOptions<SqlExecutionOptions> executionOptions)
        {
            if (connectionsFactory == null)
                throw new ArgumentNullException(nameof(connectionsFactory));
            if (executionOptions == null)
                throw new ArgumentNullException(nameof(executionOptions));

            ConnectionsFactory = connectionsFactory;
            SqlExecutionOptions = executionOptions.Value;
        }
    }
}