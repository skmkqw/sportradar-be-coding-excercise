using ErrorOr;
using MediatR;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Events.Commands.AddEvent;

public record AddEventCommand(
    string Name,
    int Season,
    string Status,
    string? Description,
    TimeSpan TimeVenueUtc,
    DateTime DateVenueUtc,
    
    Guid? ExistingHomeTeamId,
    AddTeamCommand? NewHomeTeam,
    
    Guid? ExistingAwayTeamId,
    AddTeamCommand? NewAwayTeam,
    
    Guid? ExistingStadiumId,
    AddStadiumCommand? NewStadium,
    
    Guid? ExistingStageId,
    AddStageCommand? NewStage,
    
    Guid? ExistingCompetitionId,
    AddCompetitionCommand? NewCompetition,
    
    AddResultCommand? Result
) : IRequest<ErrorOr<Event>>;

public enum TeamSide
{
    Home,
    Away
}

public record AddTeamCommand(
    string Name,
    string OfficialName, 
    string Abbreviation,
    string CountryCode,
    int? StagePosition,
    Guid SportId
);

public record AddStadiumCommand(string Name, string CountryCode);

public record AddStageCommand(string Name, int Ordering);

public record AddCompetitionCommand(string Name, Guid SportId);

public record AddResultCommand(
    List<AddPeriodScoreCommand> PeriodScores,
    List<AddIncidentCommand> Incidents,
    TeamSide? WinnerSide,
    string? Message
);

public record AddPeriodScoreCommand(int HomeScore, int AwayScore, int PeriodNumber);

public record AddIncidentCommand(string Type, TeamSide Side, int MatchMinute);