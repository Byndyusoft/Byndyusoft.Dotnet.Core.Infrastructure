namespace Byndyusoft.Extensions.Specifications.Linq
{
    public static class LinqSpecificationExtensions
    {
        public static ILinqSpecification<T> AsLinq<T>(this ISpecification<T> specification)
        {
            return (ILinqSpecification<T>)specification;
        }

        internal static bool IsSpecialCase<T>(this LinqSpecification<T> specification)
        {
            return specification.IsEmpty || specification.IsTrue || specification.IsFalse;
        }
    }
}