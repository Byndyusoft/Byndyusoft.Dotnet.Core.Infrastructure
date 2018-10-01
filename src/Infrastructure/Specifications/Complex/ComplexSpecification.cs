namespace Byndyusoft.Extensions.Specifications.Complex
{
    using System;
    using System.Linq.Expressions;
    using Linq;
    using Sql;

    public class ComplexSpecification<T> : Specification<T>, ISqlSpecification<T>, ILinqSpecification<T>
    {
        protected ComplexSpecification()
        {
        }

        public ComplexSpecification(LinqSpecification<T> linq, SqlSpecification<T> sql) : this()
        {
            Linq = linq ?? throw new ArgumentNullException(nameof(linq));
            Sql = sql ?? throw new ArgumentNullException(nameof(sql));
        }

        public LinqSpecification<T> Linq { get; }

        public SqlSpecification<T> Sql { get; }

        public virtual ComplexSpecification<T> Not()
        {
            var linq = Linq.Not();
            var sql = Sql.Not();

            return new ComplexSpecification<T>(linq, sql);
        }

        public virtual ComplexSpecification<T> And(ComplexSpecification<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            var linq = Linq.And(right.Linq);
            var sql = Sql.And(right.Sql);

            return new ComplexSpecification<T>(linq, sql);
        }

        public virtual ComplexSpecification<T> Or(ComplexSpecification<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            var linq = Linq.Or(right.Linq);
            var sql = Sql.Or(right.Sql);

            return new ComplexSpecification<T>(linq, sql);
        }

        /// <summary>
        ///  And operator
        /// </summary>
        /// <param name="left">left operand in this AND operation</param>
        /// <param name="right">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static ComplexSpecification<T> operator &(ComplexSpecification<T> left, ComplexSpecification<T> right)
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
        public static ComplexSpecification<T> operator |(ComplexSpecification<T> left, ComplexSpecification<T> right)
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
        public static ComplexSpecification<T> operator !(ComplexSpecification<T> specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));
            return specification.Not();
        }

        string ISqlSpecification<T>.ToSql() => Sql.ToSql();
        dynamic ISqlSpecification<T>.Parameters => Sql.Parameters;
        Expression<Func<T, bool>> ILinqSpecification<T>.Expression => Linq.Expression;
        Func<T, bool> ILinqSpecification<T>.Predicate => Linq.Predicate;

        internal override bool IsEmpty => Sql.IsEmpty || Linq.IsEmpty;

        internal override bool IsTrue => Sql.IsTrue || Linq.IsTrue;

        internal override bool IsFalse => Sql.IsFalse || Linq.IsFalse;
    }
}