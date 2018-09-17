namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer
{
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Infrastructure.Dapper.ConnectionsFactory;
    using Infrastructure.Logging.Serilog.Configuration;
    using Installers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Workers;

    class Program
    {
        static async Task Main(string[] args)
        {
           await new HostBuilder()
                .ConfigureHostConfiguration(
                    builder =>
                        builder
                            .AddEnvironmentVariables("SERVICE_")
                            .AddCommandLine(args)
                )
                .UseContentRoot(Path.GetDirectoryName(typeof(Program).Assembly.Location))
                .ConfigureAppConfiguration(
                    (context, builder) =>
                        builder
                            .AddEnvironmentVariables("SERVICE_")
                            .AddJsonFile("appsettings.json", false, true)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", false, true)
                )
                .ConfigureServices(
                    (context, collection) =>
                        collection
                            .AddHostedService<ConsumerWorker>()
                            .Configure<SqlConnectionsFactoryOptions>(context.Configuration.GetSection("ConnectionStrings"))
                            .Configure<RabbitConnectionsFactoryOptions>(context.Configuration.GetSection("RabbitConnectionString"))            
                )
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(
                    builder => builder.RegisterAssemblyModules(typeof(ServicesInstaller).GetTypeInfo().Assembly)
                )
                .ConfigureLogging(
                   (context, builder) =>
                        builder.AddSerilog(
                            new LoggerConfiguration()
                                .UseDefaultSettings(context.Configuration)
                                .CreateLogger()
                        )
                )
                .RunConsoleAsync();
        }
    }
}