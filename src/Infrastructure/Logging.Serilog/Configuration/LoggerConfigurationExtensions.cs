namespace Byndyusoft.Dotnet.Core.Infrastructure.Logging.Serilog.Configuration
{
    using System;
    using Enrichers;
    using global::Serilog;
    using global::Serilog.Exceptions;
    using Microsoft.Extensions.Configuration;

    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration UseDefaultSettings(
            this LoggerConfiguration loggerConfiguration,
            IConfiguration configuration)
        {
            if(loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            return loggerConfiguration
                .Enrich.WithServiceName(Environment.GetEnvironmentVariable("SERVICE_NAME"))
                .Enrich.WithApplicationInformationalVersion()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMessageTemplateHash()
                .Enrich.WithLogEventHash()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration);
        }
    }
}