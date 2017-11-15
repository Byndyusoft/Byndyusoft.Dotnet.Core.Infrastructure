namespace Byndyusoft.Dotnet.Core.Infrastructure.Rabbit
{
    using System;
    using RabbitMQ.Client;

    public interface IRabbitSession : IDisposable
    {
        IModel Channel { get; }
    }
}
