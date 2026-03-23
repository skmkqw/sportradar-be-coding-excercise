using FluentValidation;
using SportsCalendar.Domain.Enums;

namespace SportsCalendar.Application.Events.Commands.AddEvent;

public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
{
    public AddEventCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);

        // Validate EventStatus value
        RuleFor(x => x.Status)
            .IsEnumName(typeof(EventStatus))
            .WithMessage("{PropertyName} must be a valid Event Status.");


        // Ensure Home Team is provided
        RuleFor(x => x)
            .Must(x => x.ExistingHomeTeamId.HasValue || x.NewHomeTeam != null)
            .WithMessage("You must provide either an ExistingHomeTeamId or NewHomeTeam data.");

        // Ensure Away Team is provided
        RuleFor(x => x)
            .Must(x => x.ExistingAwayTeamId.HasValue || x.NewAwayTeam != null)
            .WithMessage("You must provide either an ExistingAwayTeamId or NewAwayTeam data.");

        // Ensure Competition is provided
        RuleFor(x => x)
            .Must(x => x.ExistingCompetitionId.HasValue || x.NewCompetition != null)
            .WithMessage("Competition is required.");

        // Validate MatchIncidents
        When(x => x.Result != null, () =>
        {
            RuleFor(x => x.Result!)
                .ChildRules(result =>
                {
                    // Validate the list of incidents
                    result.RuleForEach(r => r.Incidents)
                        .ChildRules(incident =>
                        {
                            // Validate IncidentType value
                            incident.RuleFor(i => i.Type)
                                .NotEmpty()
                                .IsEnumName(typeof(IncidentType))
                                .WithMessage("{PropertyName} must be a valid Incident Type.");

                            incident.RuleFor(i => i.MatchMinute)
                                .GreaterThan(0)
                                .WithMessage("Match minute must be greater than 0.");
                        });
                });
        });
    }
}