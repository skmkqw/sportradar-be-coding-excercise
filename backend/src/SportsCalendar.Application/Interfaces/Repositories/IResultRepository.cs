using System.Data;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Interfaces.Repositories;

public interface IResultRepository
{
    Task<Result?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(Result result, IDbTransaction? transaction, CancellationToken ct = default);

    Task AddPeriodScores(IEnumerable<PeriodScore> periodScores, IDbTransaction? transaction = null, CancellationToken ct = default);

    Task AddMatchIncidents(IEnumerable<MatchIncident> matchIncidents, IDbTransaction? transaction = null, CancellationToken ct = default);
}