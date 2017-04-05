namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.ExceptionsHandling
{
    using System;
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUnhandledExceptionsLoggingMiddleware(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            return applicationBuilder.UseMiddleware<UnhandledExceptionsLoggingMiddleware>();
        }
    }
}