using System.Data;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;
using SportsCalendar.Infrastructure.Extensions;

namespace SportsCalendar.Infrastructure.Repositories;

public class ResultRepository : IResultRepository
{
    private readonly IDbConnection _dbConnection;

    public ResultRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Result?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        const string sql = "SELECT * FROM Results WHERE Id = @Id";
        return await _dbConnection.QueryFirstOrDefaultWithTokenAsync<Result>(sql, new { Id = id }, ct);
    }

    public async Task AddAsync(Result result, IDbTransaction? transaction, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO Results (Id, EventId, WinnerId, Message)
            VALUES (@Id, @EventId, @WinnerId, @Message)";

        await _dbConnection.ExecuteWithTokenAsync(sql, result, transaction, ct);

        await AddPeriodScores(result.PeriodScores, transaction, ct: ct);
        await AddMatchIncidents(result.MatchIncidents, transaction, ct: ct);
    }

    public async Task AddPeriodScores(IEnumerable<PeriodScore> periodScores, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO PeriodScores (Id, ResultId, PeriodNumber, HomeScore, AwayScore)
            VALUES (@Id, @ResultId, @PeriodNumber, @HomeScore, @AwayScore)";

        await _dbConnection.ExecuteWithTokenAsync(sql, periodScores, transaction, ct);
    }

    public async Task AddMatchIncidents(IEnumerable<MatchIncident> matchIncidents, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO MatchIncidents (Id, ResultId, MatchMinute, IncidentType, TeamId)
            VALUES (@Id, @ResultId, @MatchMinute, @IncidentType, @TeamId)";

        var parameters = matchIncidents.Select(incident => new
        {
            incident.Id,
            incident.ResultId,
            incident.MatchMinute,
            IncidentType = incident.Type.ToString().ToLower(),
            incident.TeamId
        });

        await _dbConnection.ExecuteWithTokenAsync(sql, parameters, transaction, ct);
    }
}