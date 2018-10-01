namespace Byndyusoft.Extensions.Specifications
{
    using Linq;
    using Sql;

    public static class SpecificationExtensions
    {
        public static ILinqSpecification<T> AsLinq<T>(this Specification<T> specification)
        {
            return (ILinqSpecification<T>)specification;
        }

        public static ISqlSpecification<T> AsSql<T>(this Specification<T> specification)
        {
            return (ISqlSpecification<T>)specification;
        }

        internal static bool IsSpecialCase<T>(this Specification<T> specification)
        {
            return specification.IsEmpty || specification.IsTrue || specification.IsFalse;
        }
    }
}