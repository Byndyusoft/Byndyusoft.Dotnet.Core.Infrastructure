namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Infrastructure.Extensions
{
    using System;
    using Serilog;
    using Microsoft.AspNetCore.Hosting;

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseSerilog(this IWebHostBuilder hostBuilder)
        {
            if (hostBuilder == null)
                throw new ArgumentNullException(nameof(hostBuilder));

            return hostBuilder.UseSerilog(x => x.Enrich.WithApplicationInformationalVersion());
        }

        public static IWebHostBuilder UseSerilog(
            this IWebHostBuilder hostBuilder,
            Func<LoggerConfiguration, LoggerConfiguration> loggerConfigurator)
        {
            if (hostBuilder == null)
                throw new ArgumentNullException(nameof(hostBuilder));
            if (loggerConfigurator == null)
                throw new ArgumentNullException(nameof(loggerConfigurator));

            return hostBuilder.UseSerilog(
                (context, configuration) =>
                    loggerConfigurator(configuration)
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext(),
                true
            );
        }
    }
}