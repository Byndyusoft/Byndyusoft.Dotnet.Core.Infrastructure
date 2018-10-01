namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    using System;

    internal sealed class EmptySqlSpecification<T> : SqlSpecification<T>
    {
        public EmptySqlSpecification() : base(string.Empty)
        {
        }

        public override SqlSpecification<T> And(SqlSpecification<T> right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override SqlSpecification<T> Or(SqlSpecification<T> right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override SqlSpecification<T> Not()
        {
            return this;
        }

        internal override bool IsEmpty { get; } = true;
    }
}