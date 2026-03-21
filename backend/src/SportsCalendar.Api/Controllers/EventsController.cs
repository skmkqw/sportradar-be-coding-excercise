using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportsCalendar.Application.Events.Queries.GetEventById;
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

    [HttpGet("{eventId:guid}")]
    public async Task<IActionResult> GetEventById(Guid eventId)
    {
        var query = _mapper.Map<GetEventByIdQuery>(eventId);
        var getEventQueryResult = await _mediator.Send(query);

        return getEventQueryResult.Match(
            onError: errors => Problem(errors),
            onValue: _ => Ok(_mapper.Map<GetEventResponse>(getEventQueryResult.Value))
        );
    }
}