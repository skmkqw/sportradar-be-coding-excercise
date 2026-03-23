using System.Data;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Interfaces.Repositories;

public interface ICompetitionRepository
{
    Task<Competition?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(Competition competition, IDbTransaction transaction, CancellationToken ct = default);
}