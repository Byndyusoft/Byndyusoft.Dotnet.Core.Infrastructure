namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.Workers
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public abstract class RabbitWorkerBase : IHostedService
    {
        private readonly ILogger _logger;
        private readonly ManualResetEvent _waitHandle;
        private readonly TimeSpan _reconectTimeout = TimeSpan.FromSeconds(5);
        private readonly string _queueName;
        private readonly ConnectionFactory _connectionFactory;
        private readonly Thread _thread;

        protected RabbitWorkerBase(ILogger logger, IOptions<RabbitConnectionsFactoryOptions> options, string queueName)
        {
            _logger = logger;
            _connectionFactory = new ConnectionFactory
                                {
                                    Endpoint = new AmqpTcpEndpoint(options.Value.Endpoint),
                                    UserName = options.Value.UserName,
                                    Password = options.Value.Password,
                                    AutomaticRecoveryEnabled = true,
                                    NetworkRecoveryInterval = _reconectTimeout
                                };
            _waitHandle = new ManualResetEvent(false);
            _queueName = queueName;
            _thread = new Thread(Connect) { IsBackground = false };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting service...");
            _thread.Start();
            _logger.LogInformation("Service was started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping service...");
            _waitHandle.Set();
            _logger.LogInformation("Service was stopped");
            return Task.CompletedTask;
        }

            private void Connect()
        {
            var hasStop = false;
            while (hasStop == false && CreateConnection() == false)
                hasStop = _waitHandle.WaitOne(_reconectTimeout);
        }

        private bool CreateConnection()
        {
            _logger.LogInformation("Start " + GetType().Name);
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.BasicQos(0, 1000, false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += ConsumerOnReceived;
                    channel.BasicConsume(_queueName,
                        false,
                        consumer);

                    _waitHandle.WaitOne();
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Unable create rabbit mq connection");
            }
            return false;
        }

        private void ConsumerOnReceived(object consumer, BasicDeliverEventArgs ea)
        {
            const int maxNumberAttempts = 1;
            const int failDelay = 1000;

            string message = null;
            for (int i = 0; i < maxNumberAttempts; i++)
            {
                if (i > 0)
                    Thread.Sleep(failDelay);
                bool parsed = false;
                try
                {
                    message = Encoding.UTF8.GetString(ea.Body);
                    parsed = true;
                    Process(message);

                    break;
                }
                catch (Exception e)
                {
                    _logger.LogError(0, e, $"Failed proccess message {message} from queue {_queueName}, attempt # {i + 1}");

                    ResendToErrors(ea.Body, message, e.Message, parsed);
                }
            }

            try
            {
                ((IBasicConsumer)consumer).Model.BasicAck(ea.DeliveryTag, true);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, $"Failed acknowledge message {message} from queue {_queueName}");
                throw;
            }
        }

        private void ResendToErrors(byte[] body, string message, string exceptionMessage, bool parsed)
        {
            string errorQueue = "errors." + _queueName;
            byte[] errorBody;
            if (parsed)
            {
                var json = JsonConvert.SerializeObject(new
                                                       {
                                                           SourceQueue = _queueName,
                                                           ExceptionMessage = exceptionMessage,
                                                           Message = message
                                                       });
                errorBody = Encoding.UTF8.GetBytes(json);
            }
            else
                errorBody = body;

            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: errorQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                    routingKey: errorQueue,
                    basicProperties: properties,
                    body: errorBody);
            }
        }

        protected abstract void Process(string message);
    }
}