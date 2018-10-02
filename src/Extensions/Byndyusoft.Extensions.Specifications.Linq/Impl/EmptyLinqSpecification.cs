namespace Byndyusoft.Extensions.Specifications.Linq.Impl
{
    using System;

    public sealed class EmptyLinqSpecification<T> : LinqSpecification<T>
    {
        public EmptyLinqSpecification() : base(x => true)
        {
        }

        public override LinqSpecification<T> And(LinqSpecification<T> right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override LinqSpecification<T> Or(LinqSpecification<T> right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override LinqSpecification<T> Not()
        {
            return this;
        }

        public override bool IsEmpty { get; } = true;
    }
}