namespace Byndyusoft.Dotnet.Core.Infrastructure.Logging.Serilog.Enrichers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using global::Serilog;
    using global::Serilog.Configuration;

    public static class LoggerEnrichmentConfigurationExtensions
    {
        [ExcludeFromCodeCoverage]
        public static LoggerConfiguration WithApplicationVersion(
            this LoggerEnrichmentConfiguration enrichmentConfiguration, 
            string versionString)
        {
            if(enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            if(string.IsNullOrWhiteSpace(versionString))
                throw new ArgumentNullException(nameof(versionString));

            return enrichmentConfiguration.WithProperty("Version", versionString);
        }

        [ExcludeFromCodeCoverage]
        public static LoggerConfiguration WithApplicationInformationalVersion(
            this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));

            return enrichmentConfiguration.WithApplicationVersion(
                Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion
            );
        }

        [ExcludeFromCodeCoverage]
        public static LoggerConfiguration WithApplicationAssemblyVersion(
            this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));

            return enrichmentConfiguration.WithApplicationVersion(
                Assembly.GetEntryAssembly().GetName().Version.ToString(4)
            );
        }

        [ExcludeFromCodeCoverage]
        public static LoggerConfiguration WithServiceName(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            string serviceName = null)
        {
            if (enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));

            return enrichmentConfiguration.WithProperty(
                "ServiceName",
                string.IsNullOrWhiteSpace(serviceName) == false
                    ? serviceName
                    : Assembly.GetEntryAssembly().GetName().Name
            );
        }

        [ExcludeFromCodeCoverage]
        public static LoggerConfiguration WithMessageTemplateHash(
            this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));

            return enrichmentConfiguration.With<MessageTemplateHashEnricher>();
        }

        [ExcludeFromCodeCoverage]
        public static LoggerConfiguration WithLogEventHash(
            this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));

            return enrichmentConfiguration.With<LogEventHashEnricher>();
        }
    }
}