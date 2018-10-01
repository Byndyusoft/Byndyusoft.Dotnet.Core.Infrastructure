namespace Byndyusoft.Extensions.Specifications.Linq
{
    using System;
    using System.Linq.Expressions;
    using Impl;

    /// <summary>
    /// A Linq Specification is a simple implementation
    /// of specification that acquire this from a lambda expression
    /// in  constructor
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public class LinqSpecification<T> : Specification<T>, ILinqSpecification<T>
    {
        private readonly Lazy<Func<T, bool>> _predicate;

        protected internal LinqSpecification()
            : this(x => true)
        {
        }

        public LinqSpecification(Expression<Func<T, bool>> expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _predicate = new Lazy<Func<T, bool>>(expression.Compile);
        }

        /// <summary>
        /// Predicate used by this specification.
        /// </summary>
        public Func<T, bool> Predicate => _predicate.Value;

        /// <summary>
        /// Expression used by this specification.
        /// </summary>
        public Expression<Func<T, bool>> Expression { get; }

        public virtual LinqSpecification<T> And(LinqSpecification<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (right.IsSpecialCase())
                return right.And(this);

            return new AndLinqSpecification<T>(this, right);
        }

        public virtual LinqSpecification<T> Or(LinqSpecification<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (right.IsSpecialCase())
                return right.Or(this);

            return new OrLinqSpecification<T>(this, right);
        }

        public virtual LinqSpecification<T> Not()
        {
            return new NotLinqSpecification<T>(this);
        }

        /// <summary>
        ///  And operator
        /// </summary>
        /// <param name="left">left operand in this AND operation</param>
        /// <param name="right">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static LinqSpecification<T> operator &(LinqSpecification<T> left, LinqSpecification<T> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return left.And(right);
        }

        /// <summary>
        /// Or operator
        /// </summary>
        /// <param name="left">left operand in this OR operation</param>
        /// <param name="right">right operand in this OR operation</param>
        /// <returns>New specification </returns>
        public static LinqSpecification<T> operator |(LinqSpecification<T> left, LinqSpecification<T> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return left.Or(right);
        }

        /// <summary>
        /// Not specification
        /// </summary>
        /// <param name="specification">Specification to negate</param>
        /// <returns>New specification</returns>
        public static LinqSpecification<T> operator !(LinqSpecification<T> specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));
            return specification.Not();
        }
    }
}