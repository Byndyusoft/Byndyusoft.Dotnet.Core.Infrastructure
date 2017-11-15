namespace Byndyusoft.Dotnet.Core.Infrastructure.Rabbit
{
    public interface IRabbitSessionsFactory
    {
        IRabbitSession Create();
    }
}