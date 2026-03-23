using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportsCalendar.Application.Events.Commands.AddEvent;
using SportsCalendar.Application.Events.Queries.GetEventById;
using SportsCalendar.Application.Events.Queries.GetEvents;
using SportsCalendar.Contracts.Events;

namespace SportsCalendar.Api.Controllers;

[Route("api/[controller]")]
public class EventsController : BaseController
{
    private readonly IMapper _mapper;
    
    private readonly IMediator _mediator;


    public EventsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents([FromQuery] GetEventsRequest request)
    {
        var query = _mapper.Map<GetEventsQuery>(request);
        var getEventsResult = await _mediator.Send(query);

        return Ok(_mapper.Map<GetEventsResponse>(getEventsResult));
    }

    [HttpGet("{eventId:guid}")]
    public async Task<IActionResult> GetEventById(Guid eventId)
    {
        var query = _mapper.Map<GetEventByIdQuery>(eventId);
        var getEventQueryResult = await _mediator.Send(query);

        return getEventQueryResult.Match(
            onError: errors => Problem(errors),
            onValue: _ => Ok(_mapper.Map<GetFullEventResponse>(getEventQueryResult.Value))
        );
    }

    [HttpPost]
    public async Task<IActionResult> AddEvent([FromBody] CreateEventRequest request)
    {
        var command = _mapper.Map<AddEventCommand>(request);
        var createEventResult = await _mediator.Send(command);

        return createEventResult.Match(
    value => CreatedAtAction(
                actionName: nameof(GetEventById),
                routeValues: new { eventId = value.Id },
                value: _mapper.Map<GetFullEventResponse>(value)),
            errors => Problem(errors)
            );
        }
}