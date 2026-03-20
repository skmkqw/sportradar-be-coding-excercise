using ErrorOr;
using MediatR;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Events.Queries.GetEventById;

public record GetEventByIdQuery(Guid Id) : IRequest<ErrorOr<Event>>;