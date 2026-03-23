using System.Data;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Interfaces.Repositories;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id, CancellationToken ct = default);
    
    Task AddAsync(Team team, IDbTransaction transaction, CancellationToken ct = default);
}