namespace ByndyuSoft.AspNetCore.Mvc.Formatters.Yaml
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class YamlOutputFormatter : TextOutputFormatter
    {
        private readonly MvcYamlOptions _options;

        public YamlOutputFormatter(MvcYamlOptions options)
        {
            _options = options ?? new MvcYamlOptions();

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationYaml);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationXYaml);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextYaml);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextXYaml);

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (selectedEncoding == null) throw new ArgumentNullException(nameof(selectedEncoding));

            if (context.Object == null)
            {
                return;
            }

            var response = context.HttpContext.Response;
            var serializer = _options.SerializerBuilder.Build();
            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                serializer.Serialize(writer, context.Object);

                // Perf: call FlushAsync to call WriteAsync on the stream with any content left in the TextWriter's
                // buffers. This is better than just letting dispose handle it (which would result in a synchronous
                // write).
                await writer.FlushAsync();
            }
        }
    }
}