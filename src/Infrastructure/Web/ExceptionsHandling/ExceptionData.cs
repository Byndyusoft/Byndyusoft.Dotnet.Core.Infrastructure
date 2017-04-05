namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.ExceptionsHandling
{
    using System;
    using System.Text;

    public class ExceptionData 
    {
        public string ExceptionType { get; private set; }
        public string ExceptionMessage { get; private set; }
        public string StackTrace { get; private set; }
        public ExceptionData InnerException { get; private set; }

        public ExceptionData(Exception exception)
        {
            ExceptionType = exception.GetType().ToString();
            ExceptionMessage = exception.Message;
            StackTrace = exception.StackTrace;

            if (exception.InnerException != null)
                InnerException = new ExceptionData(exception.InnerException);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            var firstLine = $"{ExceptionType ?? ""} {ExceptionMessage ?? ""}".Trim();
            if (string.IsNullOrWhiteSpace(firstLine) == false)
                builder.AppendLine(firstLine);

            if (InnerException != null)
            {
                builder.AppendLine("InnerException:");
                builder.AppendLine(InnerException.ToString());
            }

            if (string.IsNullOrWhiteSpace(StackTrace) == false)
                builder.AppendLine(StackTrace);

            return builder.ToString();
        }
    }
}