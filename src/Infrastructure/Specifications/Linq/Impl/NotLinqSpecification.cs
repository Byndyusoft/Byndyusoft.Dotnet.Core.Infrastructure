namespace Byndyusoft.Extensions.Specifications.Linq.Impl
{
    using System;
    using System.Linq.Expressions;
    using Extensions;

    /// <summary>
	/// NotSpecification convert a original specification with NOT logic operator
	/// </summary>
	/// <typeparam name="T">Type of element for this specificaiton</typeparam>
    public sealed class NotLinqSpecification<T> : LinqSpecification<T>
	{
	    /// <summary>
        /// Initializes a new instance of <see cref="NotLinqSpecification{T}"/> class.
        /// </summary>
        /// <param name="inner">Original specification</param>
        public NotLinqSpecification(LinqSpecification<T> inner) 
	        : base(Expression(inner))
		{
			Inner = inner;
        }

		private LinqSpecification<T> Inner { get; }

	    public override LinqSpecification<T> Not()
	    {
	        return Inner;
	    }

	    private new static Expression<Func<T, bool>> Expression(LinqSpecification<T> inner)
	    {
	        return inner.Expression.Not();
	    }
	}
}