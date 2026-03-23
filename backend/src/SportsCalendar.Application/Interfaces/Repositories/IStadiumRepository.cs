using System.Data;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Interfaces.Repositories;

public interface IStadiumRepository
{
    Task<Stadium?> GetByIdAsync(Guid id, IDbTransaction? transaction, CancellationToken ct = default);

    Task AddAsync(Stadium stadium, IDbTransaction? transaction, CancellationToken ct = default);
}