using System.Data;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;
using SportsCalendar.Infrastructure.Extensions;

namespace SportsCalendar.Infrastructure.Repositories;

public class CompetitionRepository : ICompetitionRepository
{
    private readonly IDbConnection _dbConnection;

    public CompetitionRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Competition?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        const string sql = "SELECT * FROM Competitions WHERE Id = @Id";
        
        return await _dbConnection.QueryFirstOrDefaultWithTokenAsync<Competition>(sql, new { Id = id }, ct);
    }

    public async Task AddAsync(Competition competition, IDbTransaction transaction, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO Competitions (Id, Name, Slug, SportId)
            VALUES (@Id, @Name, @Slug, @SportId)";

        await _dbConnection.ExecuteWithTokenAsync(sql, competition, transaction, ct);
    }
}