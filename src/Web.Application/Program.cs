namespace Byndyusoft.Dotnet.Core.Samples.Web.Application
{
    using System.IO;
    using Infrastructure.Extensions;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
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
                            .AddNLogConfig($"NLog.{env.EnvironmentName}.config")
                            .AddCommandLine(args);
                    })
                .ConfigureLogging(
                    (context, builder) =>
                        builder
                            .AddConfiguration(context.Configuration.GetSection("Logging"))
                            .AddNLog()
                            .AddConsole())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
