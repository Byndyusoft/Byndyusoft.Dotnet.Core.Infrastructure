namespace ByndyuSoft.AspNetCore.Mvc.Formatters.ProtoBuf
{
    using global::ProtoBuf.Meta;

    public class MvcProtoBufOptions
    {
        public MvcProtoBufOptions()
        {
            Model = TypeModel.Create();
            Model.UseImplicitZeroDefaults = true;
        }

        public RuntimeTypeModel Model { get; set; }
    }
}