namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.ConnectionsFactory
{
    using System.Data.Common;

    public interface IDbConnectionsFactory
    {
        DbConnection Create();
    }
}