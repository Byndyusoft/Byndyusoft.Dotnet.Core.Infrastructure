namespace ByndyuSoft.AspNetCore.Mvc.Formatters.Yaml
{
    using YamlDotNet.Serialization;

    public class MvcYamlOptions
    {
        public SerializerBuilder SerializerBuilder { get; set; } = new SerializerBuilder();
        public DeserializerBuilder DeserializerBuilder { get; set; } = new DeserializerBuilder();
    }
}