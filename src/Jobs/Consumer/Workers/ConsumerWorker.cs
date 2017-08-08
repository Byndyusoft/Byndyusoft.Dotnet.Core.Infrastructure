namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.Workers
{
    using Infrastructure.CQRS.Abstractions.Commands;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [UsedImplicitly]
    public class ConsumerWorker : RabbitWorkerBase
    {
        public ConsumerWorker(ILogger<ConsumerWorker> logger, ICommandsDispatcher commandsDispatcher, IOptions<RabbitConnectionsFactoryOptions> options) 
            : base(logger, commandsDispatcher, options, "sampleQueueName")
        {
        }

        protected override void Process(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}