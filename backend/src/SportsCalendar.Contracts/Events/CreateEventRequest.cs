namespace SportsCalendar.Contracts.Events;

public record CreateEventRequest(
    string Name,
    int Season,
    string Status,
    string? Description,
    TimeSpan TimeVenueUtc,
    DateTime DateVenueUtc,

    Guid? ExistingHomeTeamId,
    CreateTeamRequest? NewHomeTeam,
    
    Guid? ExistingAwayTeamId,
    CreateTeamRequest? NewAwayTeam,

    Guid? ExistingStadiumId,
    CreateStadiumRequest? NewStadium,

    Guid? ExistingStageId,
    CreateStageRequest? NewStage,

    Guid? ExistingCompetitionId,
    CreateCompetitionRequest? NewCompetition,

    CreateResultRequest? Result
);

public record CreateTeamRequest(
    string Name,
    string OfficialName,
    string Abbreviation,
    string CountryCode,
    int? StagePosition,
    Guid SportId
);

public record CreateStadiumRequest(
    string Name,
    string CountryCode
);

public record CreateStageRequest(
    string Name,
    int Ordering
);

public record CreateCompetitionRequest(
    string Name,
    Guid SportId
);

public record CreatePeriodScoreRequest(
    int HomeScore,
    int AwayScore,
    int PeriodNumber
);

public enum TeamSide
{
    Home,
    Away
}

public record CreateIncidentRequest(
    string Type,
    TeamSide Side,
    int MatchMinute
);
public record CreateResultRequest(
    List<CreatePeriodScoreRequest> PeriodScores,
    List<CreateIncidentRequest> Incidents,
    TeamSide? WinnerSide,
    string? Message
);