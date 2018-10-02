namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Reflection;

    internal static class ExpandoObjectExtensions
    {
        public static void Add(this ExpandoObject obj, object param)
        {
            if (param == null)
                return;

            if (param is IEnumerable<KeyValuePair<string, object>> dictionary)
            {
                obj.Add(dictionary);
                return;
            }

            var expando = (IDictionary<string, object>) obj;

            var properties = param.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var property in properties)
            {
                var name = property.Name;
                var value = property.GetValue(param, null);
                expando[name] = value;
            }
        }

        public static void Add(this ExpandoObject obj, IEnumerable<KeyValuePair<string, object>> param)
        {
            var expando = (IDictionary<string, object>)obj;
            foreach (var pair in param)
            {
                expando[pair.Key] = pair.Value;
            }
        }
    }
}