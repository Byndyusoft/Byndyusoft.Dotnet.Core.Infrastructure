namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.Workers
{
    using System;
    using System.Text;
    using System.Threading;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public abstract class RabbitWorkerBase
    {
        private readonly ILogger<RabbitWorkerBase> logger;
        private readonly ManualResetEvent waitHandle;
        private readonly TimeSpan ReconectTimeout = TimeSpan.FromSeconds(5);
        private readonly string queueName;
        private readonly ConnectionFactory connectionFactory;
        private readonly Thread thread;

        protected RabbitWorkerBase(ILogger<RabbitWorkerBase> logger, IOptions<RabbitConnectionsFactoryOptions> options, string queueName)
        {
            this.logger = logger;
            connectionFactory = new ConnectionFactory
                                {
                                    Endpoint = new AmqpTcpEndpoint(options.Value.Endpoint),
                                    UserName = options.Value.UserName,
                                    Password = options.Value.Password,
                                    AutomaticRecoveryEnabled = true,
                                    NetworkRecoveryInterval = ReconectTimeout
                                };
            waitHandle = new ManualResetEvent(false);
            this.queueName = queueName;
            thread = new Thread(Connect) { IsBackground = false };
        }

        public void Start()
        {
            thread.Start();
        }

        private void Connect()
        {
            bool hasStop = false;
            while (hasStop == false && CreateConnection() == false)
            {
                hasStop = waitHandle.WaitOne(ReconectTimeout);
            }
        }

        private bool CreateConnection()
        {
            logger.LogInformation("Start " + GetType().Name);
            try
            {
                using (var connection = connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.BasicQos(0, 1000, false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += ConsumerOnReceived;
                    channel.BasicConsume(queueName,
                        false,
                        consumer);

                    waitHandle.WaitOne();
                }
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(0, e, "Unable create rabbit mq connection");
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
                    logger.LogError(0, e, $"Failed proccess message {message} from queue {queueName}, attempt # {i + 1}");

                    ResendToErrors(ea.Body, message, e.Message, parsed);
                }
            }

            try
            {
                ((IBasicConsumer)consumer).Model.BasicAck(ea.DeliveryTag, true);
            }
            catch (Exception e)
            {
                logger.LogError(0, e, $"Failed acknowledge message {message} from queue {queueName}");
                throw;
            }
        }

        private void ResendToErrors(byte[] body, string message, string exceptionMessage, bool parsed)
        {
            string errorQueue = "errors." + queueName;
            byte[] errorBody;
            if (parsed)
            {
                var json = JsonConvert.SerializeObject(new
                                                       {
                                                           SourceQueue = queueName,
                                                           ExceptionMessage = exceptionMessage,
                                                           Message = message
                                                       });
                errorBody = Encoding.UTF8.GetBytes(json);
            }
            else
                errorBody = body;

            using (var connection = connectionFactory.CreateConnection())
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