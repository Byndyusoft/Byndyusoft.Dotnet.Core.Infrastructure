namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer
{
    using System.IO;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Infrastructure.Dapper.ConnectionsFactory;
    using Infrastructure.Logging.Serilog.Configuration;
    using Installers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Workers;

    class Program
    {
        private static IConfigurationRoot _configuration;
        private static IContainer _container;

        static void Main(string[] args)
        {
            BuildConfiguration(args);
            ConfigureDependencies();

            _container.Resolve<ConsumerWorker>().Start();
        }

        private static void BuildConfiguration(string[] args)
        {
            var environmentConfiguration = new ConfigurationBuilder()
                .AddEnvironmentVariables("SERVICE_")
                .AddCommandLine(args)
                .Build();

            var environmentName = environmentConfiguration["environment"];

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", false, true)
                .Build();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        private static void ConfigureDependencies()
        {
            var services = new ServiceCollection()
                .AddLogging(
                    x => x.AddSerilog(
                        new LoggerConfiguration()
                            .UseDefaultSettings(_configuration)
                            .CreateLogger()
                    )
                )
                .AddOptions();

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