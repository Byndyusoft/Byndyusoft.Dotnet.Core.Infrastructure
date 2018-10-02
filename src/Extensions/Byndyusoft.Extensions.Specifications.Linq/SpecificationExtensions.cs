namespace Byndyusoft.Extensions.Specifications.Linq
{
    internal static class SpecificationExtensions
    {
        public static ILinqSpecification<T> AsLinq<T>(this ISpecification specification)
        {
            return (ILinqSpecification<T>)specification;
        }

        internal static bool IsSpecialCase<T>(this LinqSpecification<T> specification)
        {
            return specification.IsEmpty || specification.IsTrue || specification.IsFalse;
        }
    }
}