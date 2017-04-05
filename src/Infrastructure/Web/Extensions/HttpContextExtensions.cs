namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Authentication;

    public static class HttpContextExtensions
    {
        public static IEnumerable<AuthenticationDescription> GetExternalProviders(this HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.Authentication.GetAuthenticationSchemes()
                .Where(description => string.IsNullOrWhiteSpace(description.DisplayName) == false)
                .ToArray();
        }

        public static bool IsProviderSupported(this HttpContext context, string provider)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.GetExternalProviders().Any(description => string.Equals(description.AuthenticationScheme, provider, StringComparison.OrdinalIgnoreCase));
        }
    }
}