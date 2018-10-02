namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    using System;

    public sealed class FalseSqlSpecification : SqlSpecification
    {
        public FalseSqlSpecification() : base("1<>1")
        {
        }

        public override SqlSpecification And(SqlSpecification right)
        {
            return this;
        }

        public override SqlSpecification Or(SqlSpecification right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override SqlSpecification Not()
        {
            return new TrueSqlSpecification();
        }

        public override bool IsFalse { get; } = true;
    }
}