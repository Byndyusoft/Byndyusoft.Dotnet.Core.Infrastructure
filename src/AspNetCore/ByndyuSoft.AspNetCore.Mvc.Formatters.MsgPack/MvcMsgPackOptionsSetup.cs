namespace ByndyuSoft.AspNetCore.Mvc.Formatters.MsgPack
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    internal class MvcMsgPackOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly MvcMsgPackOptions _options;

        public MvcMsgPackOptionsSetup(IOptions<MvcMsgPackOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(MvcOptions options)
        {
            var key = "msgpack";
            var mapping = options.FormatterMappings.GetMediaTypeMappingForFormat(key);
            if (string.IsNullOrEmpty(mapping))
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat(
                    key,
                    MediaTypeHeaderValues.ApplicationMsgPack);
            }

            options.OutputFormatters.Add(new MsgPackOutputFormatter(_options));
            options.InputFormatters.Add(new MsgPackInputFormatter(_options));
        }
    }
}