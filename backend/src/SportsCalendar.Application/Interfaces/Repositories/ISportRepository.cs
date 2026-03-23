using System.Data;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Interfaces.Repositories;

public interface ISportRepository
{
    Task<Sport?> GetByIdAsync(Guid id, IDbTransaction? transaction, CancellationToken ct = default);
}