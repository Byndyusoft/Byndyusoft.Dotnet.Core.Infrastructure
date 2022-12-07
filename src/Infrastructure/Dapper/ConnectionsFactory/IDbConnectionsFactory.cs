namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.ConnectionsFactory
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Data.Common;

    public interface IDbConnectionsFactory
    {
        DbConnection Create();

        Task<DbConnection> CreateAsync(CancellationToken cancellationToken = default);
    }
}