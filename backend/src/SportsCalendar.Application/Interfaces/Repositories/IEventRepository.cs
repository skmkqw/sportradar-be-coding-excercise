using System.Data;
using SportsCalendar.Application.Common;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Interfaces.Repositories;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id, IDbTransaction? transaction = null, CancellationToken ct = default);

    Task<PagedResult<Event>> GetEventsAsync(int page = 1,
        int pageSize = 10,
        Guid? sportId = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default);

    Task<Guid> AddAsync(Event @event, IDbTransaction? transaction, CancellationToken ct = default);
}