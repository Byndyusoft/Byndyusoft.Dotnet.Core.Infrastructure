﻿namespace Byndyusoft.Extensions.Specifications.Sql
{
    using Impl;

    public partial class SqlSpecification
    {
        public static SqlSpecification Empty()
        {
            return new EmptySqlSpecification();
        }

        public static SqlSpecification True()
        {
            return new TrueSqlSpecification();
        }

        public static SqlSpecification False()
        {
            return new FalseSqlSpecification();
        }

        public static SqlSpecification Create(string where, object param = null)
        {
            return new SqlSpecification(where, param);
        }
    }
}