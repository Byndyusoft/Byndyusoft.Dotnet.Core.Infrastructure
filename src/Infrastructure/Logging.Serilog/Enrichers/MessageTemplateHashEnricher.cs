namespace Byndyusoft.Dotnet.Core.Infrastructure.Logging.Serilog.Enrichers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using global::Serilog.Core;
    using global::Serilog.Events;
    using Murmur;

    [ExcludeFromCodeCoverage]
    public class MessageTemplateHashEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var murmurHash = MurmurHash.Create32();
            var bytes = Encoding.UTF8.GetBytes(logEvent.MessageTemplate.Text);
            var hash = murmurHash.ComputeHash(bytes);
            var numericHash = BitConverter.ToUInt32(hash, 0);
            var messageTemplateHashProperty = propertyFactory.CreateProperty("MessageTemplateHash", numericHash.ToString("x8"));
            logEvent.AddPropertyIfAbsent(messageTemplateHashProperty);
        }
    }
}