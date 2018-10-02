namespace ByndyuSoft.AspNetCore.Mvc.Formatters.MsgPack
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public static class MsgPackMvcBuilderExtensions
    {
        public static IMvcBuilder AddMsgPackFormatters(this IMvcBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            AddMsgPackFormatterServices(builder.Services);
            return builder;
        }

        public static IMvcBuilder AddMsgPackFormatters(this IMvcBuilder builder, Action<MvcMsgPackOptions> setupAction)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure(setupAction);

            return AddMsgPackFormatters(builder);
        }

        public static IMvcCoreBuilder AddMsgPackFormatters(this IMvcCoreBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            AddMsgPackFormatterServices(builder.Services);
            return builder;
        }

        public static IMvcCoreBuilder AddMsgPackFormatters(this IMvcCoreBuilder builder, Action<MvcMsgPackOptions> setupAction)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure(setupAction);

            return AddMsgPackFormatters(builder);
        }

        private static void AddMsgPackFormatterServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcMsgPackOptionsSetup>());
        }
    }
}