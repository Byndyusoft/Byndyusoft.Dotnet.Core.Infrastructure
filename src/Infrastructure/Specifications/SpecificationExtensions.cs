namespace Byndyusoft.Extensions.Specifications
{
    using Linq;

    public static class SpecificationExtensions
    {
        public static ILinqSpecification<T> AsLinq<T>(this Specification<T> specification)
        {
            return (ILinqSpecification<T>)specification;
        }

        internal static bool IsSpecialCase<T>(this Specification<T> specification)
        {
            return specification.IsEmpty || specification.IsTrue || specification.IsFalse;
        }
    }
}