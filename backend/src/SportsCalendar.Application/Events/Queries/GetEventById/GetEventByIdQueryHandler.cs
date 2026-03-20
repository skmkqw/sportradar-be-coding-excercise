using ErrorOr;
using MediatR;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, ErrorOr<Event>>
{
    private readonly IEventRepository _eventRepository;

    public GetEventByIdQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<ErrorOr<Event>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result == null)
            return Error.NotFound(code: "Event.NotFound",
                description: $"Event with id: '{request.Id}' not found or doesn't exist.");

        return result;
    }
}