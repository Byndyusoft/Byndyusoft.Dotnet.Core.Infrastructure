namespace Byndyusoft.Extensions.Specifications.Complex
{
    using System;
    using System.Linq.Expressions;
    using Linq;
    using Sql;

    public class ComplexSpecification<T> : ISqlSpecification<T>, ILinqSpecification<T>
    {
        protected ComplexSpecification()
        {
        }

        public ComplexSpecification(LinqSpecification<T> linq, SqlSpecification sql) : this()
        {
            Linq = linq ?? throw new ArgumentNullException(nameof(linq));
            Sql = sql ?? throw new ArgumentNullException(nameof(sql));
        }

        public LinqSpecification<T> Linq { get; }

        public SqlSpecification Sql { get; }

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

        public string ToSql() => Sql.ToSql();
        public dynamic Parameters => Sql.Parameters;
        public Expression<Func<T, bool>> Expression => Linq.Expression;
        public Func<T, bool> Predicate => Linq.Predicate;

        public bool IsEmpty => Sql.IsEmpty || Linq.IsEmpty;

        public bool IsTrue => Sql.IsTrue || Linq.IsTrue;

        public bool IsFalse => Sql.IsFalse || Linq.IsFalse;
    }
}