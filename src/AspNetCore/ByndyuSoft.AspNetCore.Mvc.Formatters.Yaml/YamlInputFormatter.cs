namespace ByndyuSoft.AspNetCore.Mvc.Formatters.Yaml
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class YamlInputFormatter : TextInputFormatter
    {
        private readonly MvcYamlOptions _options;

        public YamlInputFormatter(MvcYamlOptions options = null)
        {
            _options = options ?? new MvcYamlOptions();

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationYaml);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationXYaml);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextYaml);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextXYaml);

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var model = ReadRequestBody(context, encoding);
            return Task.FromResult(model);   
        }

        private InputFormatterResult ReadRequestBody(InputFormatterContext context, Encoding encoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var request = context.HttpContext.Request;
            object model = null;

            if (request.ContentLength > 0)
            {
                var serializer = _options.DeserializerBuilder.Build();
                using (var reader = new StreamReader(request.Body, encoding))
                {
                    model = serializer.Deserialize(reader, context.ModelType);
                }
            }

            if (model == null && !context.TreatEmptyInputAsDefaultValue)
            {
                return InputFormatterResult.NoValue();
            }

            return InputFormatterResult.Success(model);
        }
    }
}