namespace Byndyusoft.Dotnet.Core.Samples.Web.DataAccess.Values.Repository
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class InMemoryRepository : IValuesRepository
    {
        private readonly Dictionary<int, string> _values;

        public InMemoryRepository()
        {
            _values = new Dictionary<int, string>();
        }

        public void Set(int id, string value)
        {
            _values[id] = value;
        }

        public string Get(int id)
        {
            return _values[id];
        }
    }
}