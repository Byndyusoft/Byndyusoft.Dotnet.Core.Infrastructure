namespace ByndyuSoft.AspNetCore.Mvc.Formatters.MsgPack
{
    using System.Threading;
    using System.Threading.Tasks;
    using global::MsgPack;
    using global::MsgPack.Serialization;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class MsgPackInputFormatter : InputFormatter
    {
        private readonly MvcMsgPackOptions _options;

        public MsgPackInputFormatter(MvcMsgPackOptions options = null)
        {
            _options = options ?? new MvcMsgPackOptions();

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationMsgPack);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationXMsgPack);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var serializer = MessagePackSerializer.Get(context.ModelType);
            using (var unpacker = Unpacker.Create(request.Body))
            {
                var model = await serializer.UnpackFromAsync(unpacker, CancellationToken.None);
                if (model == null && !context.TreatEmptyInputAsDefaultValue)
                {
                    return InputFormatterResult.NoValue();
                }

                return InputFormatterResult.Success(model);
            }
        }
    }
}