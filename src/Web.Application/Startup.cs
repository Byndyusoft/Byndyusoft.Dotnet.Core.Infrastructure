namespace Byndyusoft.Dotnet.Core.Samples.Web.Application
{
    using System;
    using System.IO;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Core.Infrastructure.Web.ExceptionsHandling;
    using global::Web.Application.Infrastructure.Extensions;
    using Infrastructure;
    using Infrastructure.Installers;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using NLog.Extensions.Logging;
    using NLog.Web;
    using Swashbuckle.Swagger.Model;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup
    {
        public Startup(IHostingEnvironment env, CommandLineArgumentsProvider commandLineArgumentsProvider)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(typeof(Program).GetTypeInfo().Assembly.Location))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddNLogConfig($"NLog.{env.EnvironmentName}.config")
                .AddEnvironmentVariables()
                .AddCommandLine(commandLineArgumentsProvider.Arguments);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
                                         {
                                             options
                                                 .SingleApiVersion(new Info
                                                                   {
                                                                       Version = "v1",
                                                                       Title = "Values providing API",
                                                                       Description = "A dummy to get configuration values",
                                                                       TermsOfService = "None"
                                                                   });

                                             var xmlDocsPath = Configuration.GetValue<string>("xml_docs");
                                             if (string.IsNullOrWhiteSpace(xmlDocsPath) == false)
                                                 options.IncludeXmlComments(xmlDocsPath);

                                             options.DescribeAllEnumsAsStrings();
                                         });

            services
                .AddOptions();

            services
                .AddMvc()
                .AddJsonOptions(x =>
                                {
                                    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                                    x.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                                    x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                    x.SerializerSettings.Formatting = Formatting.Indented;
                                    x.SerializerSettings.Converters.Add(new StringEnumConverter());
                                    x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                                });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterAssemblyModules(typeof(ServicesInstaller).GetTypeInfo().Assembly);
            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddNLog()
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();

            app.AddNLogWeb();

            app
                .UseUnhandledExceptionsLoggingMiddleware()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUi();
        }
    }
}
