namespace ByndyuSoft.AspNetCore.Mvc.Formatters.Yaml
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    internal class YamlMvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly MvcYamlOptions _options;

        public YamlMvcOptionsSetup(IOptions<MvcYamlOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(MvcOptions options)
        {
            var key = "yaml";
            var mapping = options.FormatterMappings.GetMediaTypeMappingForFormat(key);
            if (string.IsNullOrEmpty(mapping))
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat(
                    key,
                    MediaTypeHeaderValues.ApplicationYaml);
            }

            options.OutputFormatters.Add(new YamlOutputFormatter(_options));
            options.InputFormatters.Add(new YamlInputFormatter(_options));
        }
    }
}