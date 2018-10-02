namespace ByndyuSoft.AspNetCore.Mvc.Formatters.ProtoBuf
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using ModelBinding;

    internal class ProtoBufMvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly MvcProtoBufOptions _options;

        public ProtoBufMvcOptionsSetup(IOptions<MvcProtoBufOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(new ProtoBufDataMemberRequiredBindingMetadataProvider());

            var key = "protobuf";
            var mapping = options.FormatterMappings.GetMediaTypeMappingForFormat(key);
            if (string.IsNullOrEmpty(mapping))
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat(
                    key,
                    MediaTypeHeaderValues.ApplicationProtobuf);
            }

            options.OutputFormatters.Add(new ProtoBufOutputFormatter(_options));
            options.InputFormatters.Add(new ProtoBufInputFormatter(_options));
        }
    }
}