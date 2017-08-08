namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer
{
    using System;
    using System.IO;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Infrastructure.Dapper.ConnectionsFactory;
    using Installers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using Workers;

    class Program
    {
        private static IConfigurationRoot _configuration;
        private static IContainer _container;

        private static ILoggerFactory _loggerFactory;

        static void Main(string[] args)
        {
            BuildConfiguration();
            ConfigureDependencies();
            ConfigureLogging();

            _container.Resolve<ConsumerWorker>().Start();
        }

        private static void ConfigureLogging()
        {
            _loggerFactory
                .AddNLog()
                .AddConsole(_configuration.GetSection("Logging"))
                ;
        }

        private static void BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("SERVICE_ENVIRONMENT");

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environment}.json", false, true)
                .AddNLogConfig($"NLog.{environment}.config")
                .Build();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        private static void ConfigureDependencies()
        {
            _loggerFactory = new LoggerFactory();
            var services = new ServiceCollection()
                .AddLogging()
                .AddOptions();
            services.Add(ServiceDescriptor.Singleton(_loggerFactory));
            services.Add(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));

            services
                .Configure<SqlConnectionsFactoryOptions>(_configuration.GetSection("ConnectionStrings"))
                .Configure<RabbitConnectionsFactoryOptions>(_configuration.GetSection("RabbitConnectionString"))
                ;

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterAssemblyModules(typeof(ServicesInstaller).GetTypeInfo().Assembly);
            _container = containerBuilder.Build();
        }
    }
}