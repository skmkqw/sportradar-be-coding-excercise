using MediatR;
using SportsCalendar.Application.Common;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Events.Queries.GetEvents;

public record GetEventsQuery(
    int Page,
    int PageSize,
    DateOnly? StartDate,
    DateOnly? EndDate,
    Guid? SportId
) : IRequest<PagedResult<Event>>;