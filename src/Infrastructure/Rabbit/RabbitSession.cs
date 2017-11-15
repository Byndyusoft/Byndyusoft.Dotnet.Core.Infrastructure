namespace Byndyusoft.Dotnet.Core.Infrastructure.Rabbit
{
    using RabbitMQ.Client;

    public class RabbitSession : IRabbitSession
    {
        private readonly IConnection connection;
        public IModel Channel { get; }

        public RabbitSession(IConnection connection, IModel channel)
        {
            this.connection = connection;
            Channel = channel;
        }

        public void Dispose()
        {
            Channel.Dispose();
            connection.Dispose();
        }
    }
}