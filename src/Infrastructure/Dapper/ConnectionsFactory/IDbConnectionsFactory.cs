namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.ConnectionsFactory
{
    using System.Data;

    public interface IDbConnectionsFactory
    {
        IDbConnection Create();
    }
}