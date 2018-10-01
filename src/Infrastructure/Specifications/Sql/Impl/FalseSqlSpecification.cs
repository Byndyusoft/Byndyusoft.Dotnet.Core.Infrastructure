namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    using System;

    public sealed class FalseSqlSpecification<T> : SqlSpecification<T>
    {
        public FalseSqlSpecification() : base("1<>1")
        {
        }

        public override SqlSpecification<T> And(SqlSpecification<T> right)
        {
            return this;
        }

        public override SqlSpecification<T> Or(SqlSpecification<T> right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override SqlSpecification<T> Not()
        {
            return new TrueSqlSpecification<T>();
        }

        internal override bool IsFalse { get; } = true;
    }
}