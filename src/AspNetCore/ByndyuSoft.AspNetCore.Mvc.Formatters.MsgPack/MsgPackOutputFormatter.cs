namespace ByndyuSoft.AspNetCore.Mvc.Formatters.MsgPack
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::MsgPack;
    using global::MsgPack.Serialization;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class MsgPackOutputFormatter : OutputFormatter
    {
        private readonly MvcMsgPackOptions _options;

        public MsgPackOutputFormatter(MvcMsgPackOptions options = null)
        {
            _options = options ?? new MvcMsgPackOptions();

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationMsgPack);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationXMsgPack);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var response = context.HttpContext.Response;
            if (context.Object == null)
            {
                return;
            }

            var serializer = MessagePackSerializer.Get(context.ObjectType);
            using (var packer = Packer.Create(response.Body))
            {
                await serializer.PackToAsync(packer, context.Object, CancellationToken.None);
            }
        }
    }
}