namespace SportsCalendar.Contracts.Events;

public record GetEventResponse(
    Guid Id,
    string Name,
    string? Description,
    int Season,
    string Status,
    TimeSpan TimeVenueUtc,
    DateOnly DateVenueUtc,
    Guid HomeTeamId,
    Guid AwayTeamId,
    Guid? StadiumId,
    Guid? StageId,
    Guid CompetitionId,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);