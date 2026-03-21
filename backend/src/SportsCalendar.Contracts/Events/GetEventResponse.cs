namespace SportsCalendar.Contracts.Events;

public record GetEventResponse(
    Guid Id,
    string Name,
    string? Description,
    int Season,
    int Status,
    TimeOnly TimeVenueUtc,
    DateOnly DateVenueUtc,
    Guid HomeTeamId,
    Guid AwayTeamId,
    Guid? StadiumIGuid,
    Guid? StageId,
    Guid CompetitionId,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);