namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.Authentication.JwtBearer.UserClaimsProvider
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUserClaimsProvider
    {
        Task<Claim[]> GetClaimsAsync(string login, string password);
    }
}