namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.SessionsFactory
{
    using System.Data;

    public interface ISessionsFactory
    {
        ISession Create(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}