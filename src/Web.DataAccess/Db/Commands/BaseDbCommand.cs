namespace Byndyusoft.Dotnet.Core.Samples.Web.DataAccess.Db.Commands
{
    using System;
    using Infrastructure.Dapper;
    using Infrastructure.Dapper.SessionsFactory;
    using Microsoft.Extensions.Options;

    public abstract class BaseDbCommand
    {
        protected readonly ISessionsFactory SessionsFactory;
        protected readonly SqlExecutionOptions SqlExecutionOptions;

        protected BaseDbCommand(ISessionsFactory sessionsFactory, IOptions<SqlExecutionOptions> sqlExecutionOptions)
        {
            if (sqlExecutionOptions == null)
                throw new ArgumentNullException(nameof(sqlExecutionOptions));


            SessionsFactory = sessionsFactory ?? throw new ArgumentNullException(nameof(sessionsFactory));
            SqlExecutionOptions = sqlExecutionOptions.Value;
        }
    }
}