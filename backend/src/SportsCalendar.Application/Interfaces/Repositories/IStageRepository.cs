using System.Data;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Interfaces.Repositories;

public interface IStageRepository
{
    Task<Stage?> GetByIdAsync(Guid id, IDbTransaction? transaction, CancellationToken ct = default);

    Task AddAsync(Stage stage, IDbTransaction? transaction, CancellationToken ct = default);
}