using System.Data;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;
using SportsCalendar.Infrastructure.Extensions;

namespace SportsCalendar.Infrastructure.Repositories;

public class StadiumRepository : IStadiumRepository
{
    private readonly IDbConnection _dbConnection;

    public StadiumRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Stadium?> GetByIdAsync(Guid id, IDbTransaction? transaction, CancellationToken ct = default)
    {
        const string sql = "SELECT * FROM Stadiums WHERE Id = @Id";

        return await _dbConnection.QueryFirstOrDefaultWithTokenAsync<Stadium>(sql, transaction, new { Id = id }, ct);
    }

    public async Task AddAsync(Stadium stadium, IDbTransaction? transaction, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO Stadiums (Id, Name, CountryCode)
            VALUES (@Id, @Name, @CountryCode)";

        await _dbConnection.ExecuteWithTokenAsync(sql, stadium, transaction, ct);
    }
}