namespace Byndyusoft.Extensions.Specifications
{
    public abstract class Specification<T>
    {
        internal virtual bool IsEmpty { get; } = false;

        internal virtual bool IsTrue { get; } = false;

        internal virtual bool IsFalse { get; } = false;
    }
}
