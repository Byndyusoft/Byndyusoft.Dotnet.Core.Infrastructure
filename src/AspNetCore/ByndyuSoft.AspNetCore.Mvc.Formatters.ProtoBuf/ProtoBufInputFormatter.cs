namespace ByndyuSoft.AspNetCore.Mvc.Formatters.ProtoBuf
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using global::ProtoBuf;
    using global::ProtoBuf.Meta;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class ProtoBufInputFormatter : InputFormatter
    {
        private readonly MvcProtoBufOptions _options;

        public ProtoBufInputFormatter(MvcProtoBufOptions options = null)
        {
            _options = options ?? new MvcProtoBufOptions();

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationProtobuf);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationXProtobuf);
        }

        public RuntimeTypeModel Model => _options.Model;
        
        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var tcs = new TaskCompletionSource<InputFormatterResult>();

            try
            {
                var result = ReadRequestBody(context);
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return context.ModelType.GetCustomAttribute<ProtoContractAttribute>() != null;
        }

        private InputFormatterResult ReadRequestBody(InputFormatterContext context)
        {
            object model = Model.Deserialize(context.HttpContext.Request.Body, null, context.ModelType);

            if (model == null && !context.TreatEmptyInputAsDefaultValue)
            {
                return InputFormatterResult.NoValue();
            }

            return InputFormatterResult.Success(model);
        }
    }
}