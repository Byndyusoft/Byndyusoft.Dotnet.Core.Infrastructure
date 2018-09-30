namespace Byndyusoft.Dotnet.Core.Infrastructure.Logging.Serilog.Enrichers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using global::Serilog.Core;
    using global::Serilog.Events;
    using Murmur;

    [ExcludeFromCodeCoverage]
    public class LogEventHashEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var builder = new StringBuilder(logEvent.MessageTemplate.Text);
            for (var exception = logEvent.Exception; exception != null; exception = exception.InnerException)
            {
                builder.AppendLine(exception.GetType().AssemblyQualifiedName);
                builder.AppendLine(exception.StackTrace);
            }
            var bytes = Encoding.UTF8.GetBytes(builder.ToString());

            var hash = MurmurHash.Create32().ComputeHash(bytes);
            var numericHash = BitConverter.ToUInt32(hash, 0);

            logEvent.AddPropertyIfAbsent(
                propertyFactory.CreateProperty("LogEventHash", numericHash.ToString("x8"))
            );
        }
    }
}