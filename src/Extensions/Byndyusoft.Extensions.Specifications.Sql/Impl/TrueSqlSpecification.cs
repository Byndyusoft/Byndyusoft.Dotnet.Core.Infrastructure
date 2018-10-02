namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    using System;

    public sealed class TrueSqlSpecification : SqlSpecification
    {
        public TrueSqlSpecification() : base(string.Empty)
        {
        }

        public override SqlSpecification And(SqlSpecification right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override SqlSpecification Or(SqlSpecification right)
        {
            return this;
        }

        public override SqlSpecification Not()
        {
            return new FalseSqlSpecification();
        }

        public override bool IsTrue { get; } = true;
    }
}