namespace ByndyuSoft.AspNetCore.Mvc.Formatters.ProtoBuf
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using global::ProtoBuf;
    using global::ProtoBuf.Meta;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class ProtoBufOutputFormatter : OutputFormatter
    {
        private readonly MvcProtoBufOptions _options;

        public ProtoBufOutputFormatter(MvcProtoBufOptions options = null)
        {
            _options = options ?? new MvcProtoBufOptions();

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationProtobuf);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationXProtobuf);
        }

        public RuntimeTypeModel Model => _options.Model;

        protected override bool CanWriteType(Type type)
        {
            return type.GetCustomAttribute<ProtoContractAttribute>() != null;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var tcs = new TaskCompletionSource<object>();
            try
            {
                if (context.Object != null)
                {
                    var response = context.HttpContext.Response;
                    Model.Serialize(response.Body, context.Object);
                }

                tcs.SetResult(null);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }
    }
}