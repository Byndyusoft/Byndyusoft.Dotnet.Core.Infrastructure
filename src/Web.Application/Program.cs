namespace Byndyusoft.Dotnet.Core.Samples.Web.Application
{
    using System.IO;
    using Autofac.Extensions.DependencyInjection;
    using Core.Infrastructure.Logging.Serilog.Configuration;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
                .ConfigureServices(services => services.AddAutofac())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
                .ConfigureAppConfiguration(
                    (context, builder) =>
                    {
                        var env = context.HostingEnvironment;

                        builder
                            .AddEnvironmentVariables()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json", false, true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", false, true)
                            .AddCommandLine(args);
                    })
                .UseSerilog((context, configuration) => configuration.UseDefaultSettings(context.Configuration))
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
