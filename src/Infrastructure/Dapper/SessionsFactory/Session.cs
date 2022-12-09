namespace Byndyusoft.Dotnet.Core.Infrastructure.Dapper.SessionsFactory
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Dapper;

    public class Session : ISession
    {
        private DbConnection? _connection;
        private DbTransaction? _transaction;

        public Session(DbConnection connection, DbTransaction? transaction)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
        }

        public async IAsyncEnumerable<TSource> Query<TSource>(
            string sql, 
            object? param = null, 
            int? commandTimeout = null,
            CommandType? commandType = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var command = new CommandDefinition(sql, param, _transaction, commandTimeout, commandType,
                cancellationToken: cancellationToken);

            using var reader = await _connection.ExecuteReaderAsync(command);
            var rowParser = reader.GetRowParser<TSource>();

            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return rowParser(reader);
            }
        }

        public Task<IEnumerable<TSource>> QueryAsync<TSource>(
            string sql, 
            object? param = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null,
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var command = new CommandDefinition(sql, param, _transaction, commandTimeout, commandType,
                cancellationToken: cancellationToken);
            return _connection.QueryAsync<TSource>(command);
        }

        public Task<int> ExecuteAsync(
            string sql, 
            object? param = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null,
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var command = new CommandDefinition(sql, param, _transaction, commandTimeout, commandType,
                cancellationToken: cancellationToken);
            return _connection.ExecuteAsync(command);
        }

        public Task<TSource> ExecuteScalarAsync<TSource>(
            string sql, 
            object? param = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null,
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var command = new CommandDefinition(sql, param, _transaction, commandTimeout, commandType,
                cancellationToken: cancellationToken);
            return _connection.ExecuteScalarAsync<TSource>(command);
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public virtual void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;

            _connection?.Dispose();
            _connection = null!;

            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (_connection == null)
                throw new ObjectDisposedException(nameof(Session));
        }
    }
}