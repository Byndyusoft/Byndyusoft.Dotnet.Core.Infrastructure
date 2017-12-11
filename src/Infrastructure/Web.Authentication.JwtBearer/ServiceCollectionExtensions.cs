namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.Authentication.JwtBearer
{
    using System;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthorisationTokens(
            this IServiceCollection services,
            Action<TokensIssuingOptions> issuingOptionsConfigurator, 
            Action<JwtBearerOptions> verificationOptionsConfigurator)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (issuingOptionsConfigurator == null)
                throw new ArgumentNullException(nameof(issuingOptionsConfigurator));
            if (verificationOptionsConfigurator == null)
                throw new ArgumentNullException(nameof(verificationOptionsConfigurator));

            services
                .AddOptions()
                .Configure<TokensIssuingOptions>(issuingOptionsConfigurator)
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(verificationOptionsConfigurator);

            return services;
        }
    }
}