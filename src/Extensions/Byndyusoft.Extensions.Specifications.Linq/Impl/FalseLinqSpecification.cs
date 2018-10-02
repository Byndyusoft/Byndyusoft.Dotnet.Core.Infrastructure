namespace Byndyusoft.Extensions.Specifications.Linq.Impl
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
	/// False specification
	/// </summary>
	/// <typeparam name="T">Type of entity in this specification</typeparam>
    public sealed class FalseLinqSpecification<T> : LinqSpecification<T>
	{
        /// <summary>
        /// Initializes a new instance of <see cref="FalseLinqSpecification{T}"/> class.
        /// </summary>
		public FalseLinqSpecification() : base(Expression())
		{
		}

	    public override LinqSpecification<T> And(LinqSpecification<T> right)
	    {
	        return this;
	    }

	    public override LinqSpecification<T> Or(LinqSpecification<T> right)
	    {
	        return right ?? throw new ArgumentNullException(nameof(right));
	    }

        public override LinqSpecification<T> Not()
	    {
	        return new TrueLinqSpecification<T>();
	    }

	    public override bool IsFalse { get; } = true;

	    private new static Expression<Func<T, bool>> Expression()
	    {
	        //Create "result variable" transform adhoc execution plan in prepared plan
	        //for more info: http://geeks.ms/blogs/unai/2010/07/91/ef-4-0-performance-tips-1.aspx
            const bool result = false;
	        return t => result;
        }
    }
}