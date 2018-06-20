namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Infrastructure.Extensions
{
    using System;
    using System.Reflection;
    using Serilog;
    using Serilog.Configuration;

    public static class LoggerEnrichmentConfigurationExtensions
    {
        public static LoggerConfiguration WithApplicationVersion(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            string versionString)
        {
            if (enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            if (string.IsNullOrWhiteSpace(versionString))
                throw new ArgumentNullException(nameof(versionString));

            return enrichmentConfiguration.WithProperty("Version", versionString);
        }

        public static LoggerConfiguration WithApplicationInformationalVersion(
            this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));

            return enrichmentConfiguration.WithApplicationVersion(
                Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion
            );
        }

        public static LoggerConfiguration WithApplicationAssemblyVersion(
            this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));

            return enrichmentConfiguration.WithApplicationVersion(
                Assembly.GetEntryAssembly().GetName().Version.ToString(4)
            );
        }
    }
}