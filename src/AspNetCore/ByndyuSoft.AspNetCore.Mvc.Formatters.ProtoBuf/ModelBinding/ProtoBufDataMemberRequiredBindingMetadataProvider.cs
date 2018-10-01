namespace ByndyuSoft.AspNetCore.Mvc.Formatters.ProtoBuf
{
    using System;
    using System.Linq;
    using System.Reflection;
    using global::ProtoBuf;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

    public class ProtoBufDataMemberRequiredBindingMetadataProvider : IBindingMetadataProvider
    {
        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Types cannot be required; only properties can
            if (context.Key.MetadataKind != ModelMetadataKind.Property)
            {
                return;
            }

            if (context.BindingMetadata.IsBindingRequired)
            {
                // This value is already required, no need to look at attributes.
                return;
            }

            var protoAttribute = context
                .PropertyAttributes
                .OfType<ProtoMemberAttribute>()
                .FirstOrDefault();
            if (protoAttribute == null || protoAttribute.IsRequired == false)
            {
                return;
            }

            // isDataContract == true iff the container type has at least one ProtoContractAttribute
            var containerType = context.Key.ContainerType.GetTypeInfo();
            var isDataContract = containerType.IsDefined(typeof(ProtoContractAttribute));
            if (isDataContract)
            {
                // We don't need to add a validator, just to set IsRequired = true. The validation
                // system will do the right thing.
                context.BindingMetadata.IsBindingRequired = true;
            }
        }
    }
}