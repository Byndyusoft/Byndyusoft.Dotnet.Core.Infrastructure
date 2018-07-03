namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.Validation
{
    using System.Collections.Generic;

    public class ValidationErrorDto
    {
        public string Message { get; set; }
        public Dictionary<string, string> Data{ get; set; }
    }
}
