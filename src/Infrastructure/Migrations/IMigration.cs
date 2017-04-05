namespace Byndyusoft.Dotnet.Core.Infrastructure.Migrations
{
    public interface IMigration
    {
        long Version { get; }

        string SqlSource { get; }
    }
}