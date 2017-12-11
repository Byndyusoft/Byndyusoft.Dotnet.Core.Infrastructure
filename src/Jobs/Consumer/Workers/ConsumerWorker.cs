namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.Workers
{
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [UsedImplicitly]
    public class ConsumerWorker : RabbitWorkerBase
    {
        public ConsumerWorker(ILogger<ConsumerWorker> logger, IOptions<RabbitConnectionsFactoryOptions> options) 
            : base(logger, options, "sampleQueueName")
        {
        }

        protected override void Process(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}