namespace Byndyusoft.Extensions.Specifications.Linq.Impl
{
    using System;
    using System.Linq.Expressions;
    using Extensions;

    /// <summary>
	/// A Logic OR Specification
	/// </summary>
	/// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class OrLinqSpecification<T> : LinqSpecification<T>
	{
        /// <summary>
        /// Initializes a new instance of <see cref="OrLinqSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">Left side specification</param>
        /// <param name="right">Right side specification</param>
        public OrLinqSpecification(LinqSpecification<T> left, LinqSpecification<T> right)
	        : base(Expression(left, right))
	    {
	        Left = left;
	        Right = right;
	    }

	    public LinqSpecification<T> Left { get; }
	    public LinqSpecification<T> Right { get; }

	    private new static Expression<Func<T, bool>> Expression(LinqSpecification<T> left, LinqSpecification<T> right)
	    {
	        return left.Expression.OrElse(right.Expression);
	    }
    }
}