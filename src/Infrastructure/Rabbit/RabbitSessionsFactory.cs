namespace Byndyusoft.Dotnet.Core.Infrastructure.Rabbit
{
    using System;
    using RabbitMQ.Client;
    using Microsoft.Extensions.Options;

    public class RabbitSessionsFactory : IRabbitSessionsFactory
    {
        private readonly TimeSpan ReconectTimeout = TimeSpan.FromSeconds(5);

        private readonly IConnectionFactory connectionsFactory;

        public RabbitSessionsFactory(IOptions<RabbitConnectionsFactoryOptions> options)
        {
            connectionsFactory = new ConnectionFactory
                                 {
                                     Endpoint = new AmqpTcpEndpoint(options.Value.Endpoint),
                                     UserName = options.Value.UserName,
                                     Password = options.Value.Password,
                                     AutomaticRecoveryEnabled = true,
                                     NetworkRecoveryInterval = ReconectTimeout
                                 };
        }

        public IRabbitSession Create()
        {
            var connection = connectionsFactory.CreateConnection();
            var channel = connection.CreateModel();

            return new RabbitSession(connection, channel);
        }
    }
}