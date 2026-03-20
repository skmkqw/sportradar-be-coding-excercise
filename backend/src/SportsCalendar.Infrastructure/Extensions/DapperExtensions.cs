using System.Data;
using Dapper;

namespace SportsCalendar.Infrastructure.Extensions;

public static class DapperExtensions
{
    public static Task<T?> QueryFirstOrDefaultWithTokenAsync<T>(
        this IDbConnection db,
        string sql,
        object? parameters = null,
        CancellationToken ct = default)
    {
        var config = new CommandDefinition(
            sql,
            parameters,
            cancellationToken: ct);

        return db.QueryFirstOrDefaultAsync<T>(config);
    }

    public static Task<IEnumerable<T>> QueryWithTokenAsync<T>(
        this IDbConnection db,
        string sql,
        object? parameters = null,
        CancellationToken ct = default)
    {
        return db.QueryAsync<T>(new CommandDefinition(sql, parameters, cancellationToken: ct));
    }
}