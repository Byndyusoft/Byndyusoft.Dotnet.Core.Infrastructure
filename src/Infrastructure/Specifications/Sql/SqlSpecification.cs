namespace Byndyusoft.Extensions.Specifications.Sql
{
    using System;
    using System.Dynamic;
    using Impl;

    public class SqlSpecification<T> : Specification<T>, ISqlSpecification<T>
    {
        private readonly ExpandoObject _parameters = new ExpandoObject();

        protected SqlSpecification()
        {
        }

        public SqlSpecification(string sql) : this()
        {
            Sql = sql;
        }

        public SqlSpecification(string sql, object param) : this(sql)
        {
            _parameters.Add(param);
        }

        public string Sql { get; }

        public dynamic Parameters => _parameters;

        public string ToSql()
        {
            return string.IsNullOrWhiteSpace(Sql) ? string.Empty : $"({Sql})";
        }

        public override string ToString()
        {
            return ToSql();
        }

        public virtual SqlSpecification<T> Not()
        {
            return new NotSqlSpecification<T>(this);
        }

        public virtual SqlSpecification<T> And(SqlSpecification<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (right.IsSpecialCase())
                return right.And(this);

            return new AndSpecification<T>(this, right);
        }

        public virtual SqlSpecification<T> Or(SqlSpecification<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (right.IsSpecialCase())
                return right.Or(this);

            return new OrSqlSpecification<T>(this, right);
        }

        /// <summary>
        ///  And operator
        /// </summary>
        /// <param name="left">left operand in this AND operation</param>
        /// <param name="right">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static SqlSpecification<T> operator &(SqlSpecification<T> left, SqlSpecification<T> right)
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
        public static SqlSpecification<T> operator |(SqlSpecification<T> left, SqlSpecification<T> right)
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
        public static SqlSpecification<T> operator !(SqlSpecification<T> specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));
            return specification.Not();
        }
    }
}
