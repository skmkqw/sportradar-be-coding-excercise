namespace SportsCalendar.Contracts.Events;

public record TeamResponse(
    Guid Id,
    string Name,
    string OfficialName,
    string Slug,
    string Abbreviation,
    string CountryCode,
    Guid SportId,
    int? StagePosition,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record StadiumResponse(
    Guid Id,
    string Name,
    string CountryCode,
    DateTime CreatedAtUtc
);

public record StageResponse(
    Guid Id,
    string Name,
    int? Ordering,
    DateTime CreatedAtUtc
);

public record CompetitionResponse(
    Guid Id,
    string Name,
    string Slug,
    Guid SportId,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record PeriodScoreResponse(
    Guid Id,
    int PeriodNumber,
    int HomeScore,
    int AwayScore,
    DateTime CreatedAtUtc
);

public record MatchIncidentResponse(
    Guid Id,
    int PeriodNumber,
    Guid TeamId,
    string Type,
    int MatchMinute,
    DateTime CreatedAtUtc
);

public record ResultResponse(
    Guid Id,
    int HomeScore,
    int AwayScore,
    PeriodScoreResponse[] PeriodScores,
    MatchIncidentResponse[] MatchIncidents,
    Guid? WinnerId,
    string? Message,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record GetFullEventResponse(
    Guid Id,
    string Name,
    string? Description,
    int Season,
    string Status,
    TimeSpan TimeVenueUtc,
    DateOnly DateVenueUtc,
    TeamResponse HomeTeam,
    TeamResponse AwayTeam,
    StadiumResponse Stadium,
    StageResponse? Stage,
    CompetitionResponse Competition,
    ResultResponse? Result,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);