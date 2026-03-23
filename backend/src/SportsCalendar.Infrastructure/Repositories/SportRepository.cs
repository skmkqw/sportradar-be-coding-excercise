using System.Data;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;
using SportsCalendar.Infrastructure.Extensions;

namespace SportsCalendar.Infrastructure.Repositories;

public class SportRepository : ISportRepository
{
    private readonly IDbConnection _dbConnection;

    public SportRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Sport?> GetByIdAsync(Guid id, IDbTransaction? transaction, CancellationToken ct = default)
    {
        const string sql = "SELECT * FROM Sports WHERE Id = @Id";

        return await _dbConnection.QueryFirstOrDefaultWithTokenAsync<Sport>(sql, transaction, new { Id = id }, ct);
    }
}