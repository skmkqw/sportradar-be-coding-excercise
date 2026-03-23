using System.Data;
using SportsCalendar.Application.Interfaces;

namespace SportsCalendar.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction;

    public UnitOfWork(IDbConnection connection)
    {
        _connection = connection;
    }

    public IDbConnection Connection => _connection;
    public IDbTransaction? Transaction => _transaction;

    public void Begin()
    {
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        _transaction = _connection.BeginTransaction();
    }
    public void Commit() { _transaction?.Commit(); DisposeTransaction(); }
    public void Rollback() { _transaction?.Rollback(); DisposeTransaction(); }

    private void DisposeTransaction() { _transaction?.Dispose(); _transaction = null; }
    public void Dispose() { DisposeTransaction(); _connection.Dispose(); }
}