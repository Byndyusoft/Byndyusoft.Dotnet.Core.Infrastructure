namespace ByndyuSoft.AspNetCore.Mvc.Formatters.ProtoBuf
{
    using Microsoft.Net.Http.Headers;

    internal static class MediaTypeHeaderValues
    {
        public static MediaTypeHeaderValue ApplicationProtobuf = new MediaTypeHeaderValue("application/protobuf");
        public static MediaTypeHeaderValue ApplicationXProtobuf = new MediaTypeHeaderValue("application/x-protobuf");
    }
}