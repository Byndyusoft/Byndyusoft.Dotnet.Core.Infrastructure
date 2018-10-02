namespace Byndyusoft.Extensions.Specifications.Sql.Impl
{
    using System;

    internal sealed class EmptySqlSpecification : SqlSpecification
    {
        public EmptySqlSpecification() : base(string.Empty)
        {
        }

        public override SqlSpecification And(SqlSpecification right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override SqlSpecification Or(SqlSpecification right)
        {
            return right ?? throw new ArgumentNullException(nameof(right));
        }

        public override SqlSpecification Not()
        {
            return this;
        }

        public override bool IsEmpty { get; } = true;
    }
}