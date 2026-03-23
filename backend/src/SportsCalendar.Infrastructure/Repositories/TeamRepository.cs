using System.Data;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;
using SportsCalendar.Infrastructure.Extensions;

namespace SportsCalendar.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly IDbConnection _dbConnection;

    public TeamRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Team?> GetByIdAsync(Guid id, IDbTransaction? transaction, CancellationToken ct = default)
    {
        const string sql = "SELECT * FROM Teams WHERE Id = @Id";

        return await _dbConnection.QueryFirstOrDefaultWithTokenAsync<Team>(sql, transaction, new { Id = id }, ct);
    }

    public async Task AddAsync(Team team, IDbTransaction? transaction, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId, StagePosition)
            VALUES (@Id, @Name, @OfficialName, @Slug, @Abbreviation, @CountryCode, @SportId, @StagePosition)";

        await _dbConnection.ExecuteWithTokenAsync(sql, team, transaction, ct);
    }
}