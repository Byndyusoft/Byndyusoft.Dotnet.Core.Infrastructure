namespace ByndyuSoft.AspNetCore.Mvc.Formatters.MsgPack
{
    using Microsoft.Net.Http.Headers;

    internal static class MediaTypeHeaderValues
    {
        public static MediaTypeHeaderValue ApplicationMsgPack = new MediaTypeHeaderValue("application/msgpack");
        public static MediaTypeHeaderValue ApplicationXMsgPack = new MediaTypeHeaderValue("application/x-msgpack");
    }
}
