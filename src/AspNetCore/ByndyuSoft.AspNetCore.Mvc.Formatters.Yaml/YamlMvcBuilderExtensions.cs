namespace ByndyuSoft.AspNetCore.Mvc.Formatters.Yaml
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public static class YamlMvcBuilderExtensions
    {
        public static IMvcBuilder AddYamlFormatters(this IMvcBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            AddYamlFormatterServices(builder.Services);
            return builder;
        }

        public static IMvcBuilder AddYamlFormatters(this IMvcBuilder builder, Action<MvcYamlOptions> setupAction)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure(setupAction);

            return AddYamlFormatters(builder);
        }

        public static IMvcCoreBuilder AddYamlFormatters(this IMvcCoreBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            AddYamlFormatterServices(builder.Services);
            return builder;
        }

        public static IMvcCoreBuilder AddYamlFormatters(this IMvcCoreBuilder builder, Action<MvcYamlOptions> setupAction)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure(setupAction);

            return AddYamlFormatters(builder);
        }

        private static void AddYamlFormatterServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, YamlMvcOptionsSetup>());
        }
    }
}