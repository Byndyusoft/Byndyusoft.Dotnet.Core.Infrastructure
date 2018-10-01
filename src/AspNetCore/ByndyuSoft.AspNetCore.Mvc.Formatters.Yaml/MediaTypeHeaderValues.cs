namespace ByndyuSoft.AspNetCore.Mvc.Formatters.Yaml
{
    using Microsoft.Net.Http.Headers;

    internal static class MediaTypeHeaderValues
    {
        public static MediaTypeHeaderValue TextYaml = new MediaTypeHeaderValue("text/yaml");
        public static MediaTypeHeaderValue TextXYaml = new MediaTypeHeaderValue("text/x-yaml");
        public static MediaTypeHeaderValue ApplicationYaml = new MediaTypeHeaderValue("application/yaml");
        public static MediaTypeHeaderValue ApplicationXYaml = new MediaTypeHeaderValue("application/x-yaml");
    }
}
