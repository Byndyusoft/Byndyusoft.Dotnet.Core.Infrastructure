namespace Byndyusoft.Dotnet.Core.Samples.Web.DataAccess.Values.Repository
{
    public interface IValuesRepository
    {
        void Set(int id, string value);

        string Get(int id);
    }
}