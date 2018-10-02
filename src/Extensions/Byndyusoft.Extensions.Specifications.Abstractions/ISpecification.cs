namespace Byndyusoft.Extensions.Specifications
{
    public interface ISpecification
    {
        bool IsEmpty { get; }
        bool IsTrue { get; }
        bool IsFalse { get; }
    }

    public interface ISpecification<T>
    {
    }
}