namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.DbTypes
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using global::Dapper;
    using Microsoft.SqlServer.Server;

    public class Int32IdsList : SqlMapper.ICustomQueryParameter
    {
        private readonly IEnumerable<int> _ids;

        public Int32IdsList(IEnumerable<int> ids)
        {
            _ids = ids;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.Parameters.Add(GetParameter(name));
        }

        private SqlParameter GetParameter(string name)
        {
            var numberList = new List<SqlDataRecord>();

            var tvpDefinition = new[] { new SqlMetaData("id", SqlDbType.Int) };

            foreach (var id in _ids)
            {
                var rec = new SqlDataRecord(tvpDefinition);
                rec.SetInt32(0, id);
                numberList.Add(rec);
            }

            return new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.Structured,
                Direction = ParameterDirection.Input,
                TypeName = "Int32IdsList",
                Value = numberList
            };
        }
    }
}