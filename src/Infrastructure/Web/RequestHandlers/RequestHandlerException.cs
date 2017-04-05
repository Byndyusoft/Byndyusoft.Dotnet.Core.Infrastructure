namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.RequestHandlers
{
    using System;

    public class RequestHandlerException : InvalidOperationException
    {
        public string[] Errors { get; private set; }

        public RequestHandlerException(params string[] errors)
        {
            Errors = errors;
        }

        public RequestHandlerException() : this(new string[0])
        {
        }
    }
}