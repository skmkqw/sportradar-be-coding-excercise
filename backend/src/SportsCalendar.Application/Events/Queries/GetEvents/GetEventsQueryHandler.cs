using MediatR;
using Microsoft.Extensions.Logging;
using SportsCalendar.Application.Common;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Events.Queries.GetEvents;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, PagedResult<Event>>
{
    private readonly IEventRepository _eventRepository;

    private readonly ILogger<GetEventsQueryHandler> _logger;

    public GetEventsQueryHandler(IEventRepository eventRepository, ILogger<GetEventsQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<PagedResult<Event>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching a list of events...");

        var result = await _eventRepository.GetEventsAsync(request.Page,
            request.PageSize,
            request.SportId,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        _logger.LogInformation($"Successfully fetched ${result.Items.Count} events.");

        return result;
    }
}