namespace Byndyusoft.Extensions.Specifications.Sql
{
    using System;
    using System.Dynamic;
    using Impl;

    public partial class SqlSpecification : ISqlSpecification
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

        public virtual SqlSpecification Not()
        {
            return new NotSqlSpecification(this);
        }

        public virtual SqlSpecification And(SqlSpecification right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (right.IsSpecialCase())
                return right.And(this);

            return new AndSpecification(this, right);
        }

        public virtual SqlSpecification Or(SqlSpecification right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (right.IsSpecialCase())
                return right.Or(this);

            return new OrSqlSpecification(this, right);
        }

        /// <summary>
        ///  And operator
        /// </summary>
        /// <param name="left">left operand in this AND operation</param>
        /// <param name="right">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static SqlSpecification operator &(SqlSpecification left, SqlSpecification right)
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
        public static SqlSpecification operator |(SqlSpecification left, SqlSpecification right)
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
        public static SqlSpecification operator !(SqlSpecification specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));
            return specification.Not();
        }

        public virtual bool IsEmpty { get; } = false;

        public virtual bool IsTrue { get; } = false;

        public virtual bool IsFalse { get; } = false;
    }
}
