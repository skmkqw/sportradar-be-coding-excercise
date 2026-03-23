using System.Data;
using Dapper;

namespace SportsCalendar.Infrastructure.Extensions;

public static class DapperExtensions
{
    public static Task<T?> QueryFirstOrDefaultWithTokenAsync<T>(
        this IDbConnection db,
        string sql,
        IDbTransaction? transaction,
        object? parameters = null,
        CancellationToken ct = default)
    {
        var config = new CommandDefinition(
            sql,
            parameters,
            transaction,
            cancellationToken: ct);

        return db.QueryFirstOrDefaultAsync<T>(config);
    }

    public static Task<IEnumerable<T>> QueryWithTokenAsync<T>(
        this IDbConnection db,
        string sql,
        IDbTransaction? transaction,
        object? parameters = null,
        CancellationToken ct = default)
    {
        return db.QueryAsync<T>(new CommandDefinition(sql, parameters, transaction, cancellationToken: ct));
    }

    public static Task ExecuteWithTokenAsync<T>(this IDbConnection db,
        string sql,
        T data,
        IDbTransaction? transaction,
        CancellationToken ct = default)
    {
        return db.ExecuteAsync(new CommandDefinition(sql, data, transaction, cancellationToken: ct));
    }

    public static Task ExecuteWithTokenAsync<T>(this IDbConnection db,
        string sql,
        IEnumerable<T> data,
        IDbTransaction? transaction,
        CancellationToken ct = default)
    {
        return db.ExecuteAsync(new CommandDefinition(sql, data, transaction, cancellationToken: ct));
    }
}