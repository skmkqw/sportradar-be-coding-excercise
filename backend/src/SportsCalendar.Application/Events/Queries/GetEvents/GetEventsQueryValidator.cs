using FluentValidation;

namespace SportsCalendar.Application.Events.Queries.GetEvents;

public class GetEventsQueryValidator : AbstractValidator<GetEventsQuery>
{
    public GetEventsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater that 0");
        
        RuleFor(x => x.PageSize)
            .ExclusiveBetween(1, 20)
            .WithMessage("Page size value must be between 1 and 20.");
    }
}