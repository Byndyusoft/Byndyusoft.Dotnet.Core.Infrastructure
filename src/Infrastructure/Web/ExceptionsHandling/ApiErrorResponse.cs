namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.ExceptionsHandling
{
    using System;
    using System.Text;

    public class ApiErrorResponse : ExceptionData
    {
        public string Message { get; private set; }

        public ApiErrorResponse(string message, Exception exception) : base(exception)
        {
            Message = message;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Message) == false)
                builder.AppendLine(Message);

            var baseInformation = base.ToString();
            if (string.IsNullOrWhiteSpace(baseInformation) == false)
                builder.AppendLine(baseInformation);

            return builder.ToString();
        }
    }
}