namespace Byndyusoft.Dotnet.Core.Samples.Web.Application
{
    using System;
    using System.Reflection;
    using Autofac;
    using ByndyuSoft.AspNetCore.Mvc.Formatters.ProtoBuf;
    using ByndyuSoft.AspNetCore.Mvc.Formatters.Yaml;
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
    using Swashbuckle.AspNetCore.Swagger;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly string _versionString;

        public Startup(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration;
            _versionString = GetType().Assembly.GetName().Version.ToString(4);
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
                                   {
                                       options
                                           .SwaggerDoc($"v{_versionString}",
                                               new Info
                                               {
                                                   Version = $"v{_versionString}",
                                                   Title = "Values providing API",
                                                   Description = "A dummy to get configuration values",
                                                   TermsOfService = "None"
                                               });

                                       var xmlDocsPath = _configuration.GetValue<string>("xml_docs");
                                       if (string.IsNullOrWhiteSpace(xmlDocsPath) == false)
                                           options.IncludeXmlComments(xmlDocsPath);

                                       options.DescribeAllEnumsAsStrings();
                                   });

            services
                .AddOptions()
                .Configure<ValuesControllerOptions>(_configuration.GetSection(nameof(ValuesControllerOptions)));

            services
                .AddMvc()
                .AddProtoBufFormatters()
                .AddYamlFormatters()
                .AddXmlSerializerFormatters()
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
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyModules(typeof(ServicesInstaller).GetTypeInfo().Assembly);
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseUnhandledExceptionsLoggingMiddleware()
                .UseSwagger()
                .UseSwaggerUI(options =>
                              {
                                  options.SwaggerEndpoint($"/swagger/v{_versionString}/swagger.json", $"Values providing API v{_versionString}");
                              })
                .UseMvc();
        }
    }
}
