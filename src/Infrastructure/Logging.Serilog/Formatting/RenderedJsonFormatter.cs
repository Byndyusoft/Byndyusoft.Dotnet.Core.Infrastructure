namespace Byndyusoft.Dotnet.Core.Infrastructure.Logging.Serilog.Formatting
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using global::Serilog.Formatting.Json;

    [ExcludeFromCodeCoverage]
    public class RenderedJsonFormatter : JsonFormatter
    {
        public RenderedJsonFormatter(string closingDelimiter = null, IFormatProvider formatProvider = null) :
            base(closingDelimiter, true, formatProvider)
        {
        }
    }
}