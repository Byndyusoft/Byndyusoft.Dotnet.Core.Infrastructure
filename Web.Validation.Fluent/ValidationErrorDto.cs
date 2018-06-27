using System.Collections.Generic;

namespace Web.Validation
{
    public class ValidationErrorDto
    {
        public string Message { get; set; }
        public Dictionary<string, string> Data{ get; set; }
    }
}
