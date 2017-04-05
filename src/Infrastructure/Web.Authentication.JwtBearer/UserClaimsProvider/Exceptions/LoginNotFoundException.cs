namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.Authentication.JwtBearer.UserClaimsProvider.Exceptions
{
    using System;

    public class LoginNotFoundException : Exception
    {
        public LoginNotFoundException(string login) : base($"Login {login} wasn't found")
        {
        }
    }
}