using FluentValidation;

namespace SportsCalendar.Application.Events.Queries.GetEventById;

public class GetEventByIdQueryValidator : AbstractValidator<GetEventByIdQuery>
{
    public GetEventByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Event Id is required.");
    }
}