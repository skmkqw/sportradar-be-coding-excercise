using SportsCalendar.Domain.Models;

namespace SportsCalendar.Infrastructure.Repositories.Events;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IEnumerable<Event>> GetAllAsync(int page = 1,
        int pageSize = 10,
        string? sportSlug = null,
        DateTime? date = null,
        CancellationToken ct = default);
    
    Task<Guid> AddAsync(Event @event, CancellationToken ct = default);
}