namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.Workers
{
    using System;
    using System.Text;
    using System.Threading;
    using Infrastructure.CQRS.Abstractions.Commands;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public abstract class RabbitWorkerBase
    {
        protected ILogger<RabbitWorkerBase> Logger;
        protected ICommandsDispatcher _commandsDispatcher;
        protected ManualResetEvent waitHandle;
        protected bool aborted;
        private readonly TimeSpan ReconectTimeout = TimeSpan.FromSeconds(5);
        protected string queue;
        protected bool resendErrors;
        private ConnectionFactory ConnectionFactory;
        private Thread thread;

        protected RabbitWorkerBase(ILogger<RabbitWorkerBase> logger, ICommandsDispatcher commandsDispatcher, IOptions<RabbitConnectionsFactoryOptions> options, string queue)
        {
            Logger = logger;
            _commandsDispatcher = commandsDispatcher;
            ConnectionFactory = new ConnectionFactory
                                {
                                    Endpoint = new AmqpTcpEndpoint(options.Value.Endpoint),
                                    UserName = options.Value.UserName,
                                    Password = options.Value.Password,
                                    AutomaticRecoveryEnabled = true,
                                    NetworkRecoveryInterval = ReconectTimeout
                                };
            waitHandle = new ManualResetEvent(false);
            this.queue = queue;
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
            Logger.LogInformation("Start " + GetType().Name);
            try
            {
                using (var connection = ConnectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.BasicQos(0, 1000, false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += ConsumerOnReceived;
                    channel.BasicConsume(queue: queue,
                        noAck: false,
                        consumer: consumer);

                    waitHandle.WaitOne();
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(0, e, "Unable create rabbit mq connection");
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
                    Logger.LogError(0, e, $"Failed proccess message {message} from queue {queue}, attempt # {i + 1}");

                    ResendToErrors(ea.Body, message, e.Message, parsed);
                }
            }

            if (aborted)
                return;

            try
            {
                ((IBasicConsumer)consumer).Model.BasicAck(ea.DeliveryTag, true);
            }
            catch (Exception e)
            {
                Logger.LogError(0, e, $"Failed acknowledge message {message} from queue {queue}");
                throw;
            }
        }

        private void ResendToErrors(byte[] body, string message, string exceptionMessage, bool parsed)
        {
            string errorQueue = "errors." + queue;
            byte[] errorBody;
            if (parsed)
            {
                var json = JsonConvert.SerializeObject(new
                                                       {
                                                           SourceQueue = queue,
                                                           ExceptionMessage = exceptionMessage,
                                                           Message = message
                                                       });
                errorBody = Encoding.UTF8.GetBytes(json);
            }
            else
                errorBody = body;

            using (var connection = ConnectionFactory.CreateConnection())
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