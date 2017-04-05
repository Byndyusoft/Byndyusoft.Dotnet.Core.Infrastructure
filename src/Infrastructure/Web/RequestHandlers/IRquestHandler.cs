namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.RequestHandlers
{
    using System.Threading.Tasks;

    public interface IRquestHandler<in TRequest>
    {
        Task Handle(TRequest request);
    }
}