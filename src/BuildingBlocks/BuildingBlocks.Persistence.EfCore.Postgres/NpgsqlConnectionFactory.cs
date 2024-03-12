using System.Data;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.Persistence.EfCore;
using Npgsql;

namespace BuildingBlocks.Persistence.EfCore.Postgres;

public class NpgsqlConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;
    private IDbConnection? _connection;

    public NpgsqlConnectionFactory(string connectionString)
    {
        Guard.Against.NullOrEmpty(
            connectionString,
            nameof(connectionString),
            "ConnectionString can't be empty or null.");
        _connectionString = connectionString;
    }

    public IDbConnection GetOrCreateConnection()
    {
        if (_connection is null || _connection.State != ConnectionState.Open)
        {
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
        }

        return _connection;
    }

    public void Dispose()
    {
        if (_connection is {State: ConnectionState.Open})
            _connection.Dispose();
    }
}
