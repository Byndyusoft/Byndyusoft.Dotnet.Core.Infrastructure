namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    using System;

    public sealed class TrueSqlSpecification<T> : SqlSpecification<T>
    {
        public TrueSqlSpecification() : base(string.Empty)
        {
        }

        public override SqlSpecification<T> And(SqlSpecification<T> right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override SqlSpecification<T> Or(SqlSpecification<T> right)
        {
            return this;
        }

        public override SqlSpecification<T> Not()
        {
            return new FalseSqlSpecification<T>();
        }

        internal override bool IsTrue { get; } = true;
    }
}