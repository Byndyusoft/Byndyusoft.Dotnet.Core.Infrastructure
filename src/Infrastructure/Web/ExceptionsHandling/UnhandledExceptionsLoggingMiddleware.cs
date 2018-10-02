namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.ExceptionsHandling
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public class UnhandledExceptionsLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public UnhandledExceptionsLoggingMiddleware(RequestDelegate next, IHostingEnvironment hostingEnvironment,
            ILogger<UnhandledExceptionsLoggingMiddleware> logger)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _next = next;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;

            _jsonSerializerSettings = new JsonSerializerSettings
                                      {
                                          NullValueHandling = NullValueHandling.Ignore,
                                          DefaultValueHandling = DefaultValueHandling.Ignore,
                                          ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                          Formatting = Formatting.Indented,
                                          Converters = {new StringEnumConverter()},
                                          DateTimeZoneHandling = DateTimeZoneHandling.Utc
                                      };
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Request was cancelled");

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            catch (Exception exception)
            {
                context.Response.Clear();

                const string message = "Unhandled exception has occurred";
                _logger.LogError(0, exception, message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (_hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsStaging())
                {
                    var content = new ApiErrorResponse(message, exception);

                    var outputFormatterSelector = context.RequestServices.GetService<OutputFormatterSelector>();
                    var writersFactory = context.RequestServices.GetService<IHttpResponseStreamWriterFactory>();
                    if (outputFormatterSelector != null && writersFactory != null)
                    {
                        var formatterContext = new OutputFormatterWriteContext(
                            context,
                            writersFactory.CreateWriter,
                            content.GetType(),
                            content
                        );
                        var selectedFormatter = outputFormatterSelector.SelectFormatter(
                            formatterContext,
                            new List<IOutputFormatter>(),
                            new MediaTypeCollection()
                        );

                        if (selectedFormatter != null)
                            await selectedFormatter.WriteAsync(formatterContext);
                    }
                    else
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject(content, _jsonSerializerSettings)
                        );
                    }
                }
            }
        }
    }
}