namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.SessionsFactory
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using global::Dapper;

    public class Session : ISession
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public Session(IDbConnection connection, IDbTransaction transaction)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<TSource> Query<TSource>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.Query<TSource>(sql, param, _transaction, buffered, commandTimeout, commandType);
        }

        public Task<IEnumerable<TSource>> QueryAsync<TSource>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.QueryAsync<TSource>(sql, param, _transaction, commandTimeout, commandType);
        }

        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.Execute(sql, param, _transaction, commandTimeout, commandType);
        }

        public Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.ExecuteAsync(sql, param, _transaction, commandTimeout, commandType);
        }

        public TSource ExecuteScalar<TSource>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.ExecuteScalar<TSource>(sql, param, _transaction, commandTimeout, commandType);
        }

        public Task<TSource> ExecuteScalarAsync<TSource>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.ExecuteScalarAsync<TSource>(sql, param, _transaction, commandTimeout, commandType);
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
            _connection.Dispose();
        }
    }
}