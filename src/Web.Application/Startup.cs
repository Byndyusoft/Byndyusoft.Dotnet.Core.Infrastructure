namespace Byndyusoft.Dotnet.Core.Samples.Web.Application
{
    using System;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Controllers.ValuesController;
    using Core.Infrastructure.Web.ExceptionsHandling;
    using Infrastructure.Installers;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using NLog.Web;
    using Swashbuckle.Swagger.Model;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            Configuration = configuration;
        }

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
                .AddOptions()
                .Configure<ValuesControllerOptions>(Configuration.GetSection(nameof(ValuesControllerOptions)));

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
                                    x.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                                });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterAssemblyModules(typeof(ServicesInstaller).GetTypeInfo().Assembly);
            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.AddNLogWeb();

            app
                .UseUnhandledExceptionsLoggingMiddleware()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUi();
        }
    }
}
