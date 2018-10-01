namespace ByndyuSoft.AspNetCore.Mvc.Formatters.ProtoBuf
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public static class ProtoBufMvcBuilderExtensions
    {
        public static IMvcBuilder AddProtoBufFormatters(this IMvcBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            AddProtoBufFormatterServices(builder.Services);
            return builder;
        }

        public static IMvcBuilder AddProtoBufFormatters(this IMvcBuilder builder, Action<MvcProtoBufOptions> setupAction)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure(setupAction);

            return AddProtoBufFormatters(builder);
        }

        public static IMvcCoreBuilder AddProtoBufFormatters(this IMvcCoreBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            AddProtoBufFormatterServices(builder.Services);
            return builder;
        }

        public static IMvcCoreBuilder AddProtoBufFormatters(this IMvcCoreBuilder builder, Action<MvcProtoBufOptions> setupAction)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure(setupAction);

            return AddProtoBufFormatters(builder);
        }

        private static void AddProtoBufFormatterServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, ProtoBufMvcOptionsSetup>());
        }
    }
}