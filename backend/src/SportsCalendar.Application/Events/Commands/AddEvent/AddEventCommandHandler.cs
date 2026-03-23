using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using SportsCalendar.Application.Interfaces;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Enums;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Application.Events.Commands.AddEvent;

public class AddEventCommandHandler : IRequestHandler<AddEventCommand, ErrorOr<Event>>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IEventRepository _eventRepository;

    private readonly ITeamRepository _teamRepository;

    private readonly IStadiumRepository _stadiumRepository;

    private readonly IStageRepository _stageRepository;

    private readonly ICompetitionRepository _competitionRepository;

    private readonly IResultRepository _resultRepository;

    private readonly ILogger<AddEventCommandHandler> _logger;

    public AddEventCommandHandler(
        IUnitOfWork unitOfWork, 
        IEventRepository eventRepository,
        ITeamRepository teamRepository,
        IStadiumRepository stadiumRepository,
        IStageRepository stageRepository,
        ICompetitionRepository competitionRepository,
        IResultRepository resultRepository, 
        ILogger<AddEventCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
        _teamRepository = teamRepository;
        _stadiumRepository = stadiumRepository;
        _stageRepository = stageRepository;
        _competitionRepository = competitionRepository;
        _resultRepository = resultRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<Event>> Handle(AddEventCommand request, CancellationToken ct)
    {
        _unitOfWork.Begin();
        try
        {
            // TODO: Verify that the same SportId was provided in each model
            // Home Team
            var resolveHomeTeamResult = await ResolveTeam(request, TeamSide.Home, ct);

            if (resolveHomeTeamResult.IsError)
                return resolveHomeTeamResult.Errors;
            
            Team homeTeam = resolveHomeTeamResult.Value;

            // Away Team
            var resolveAwayTeamResult = await ResolveTeam(request, TeamSide.Away, ct);

            if (resolveAwayTeamResult.IsError)
                return resolveAwayTeamResult.Errors;

            Team awayTeam = resolveAwayTeamResult.Value;

            // Competition
            var resolveCompetitionResult = await ResolveCompetition(request, ct);

            if (resolveCompetitionResult.IsError)
                return resolveCompetitionResult.Errors;

            Competition competition = resolveCompetitionResult.Value;

            // Stadium
            var resolveStadiumResult = await ResolveStadium(request, ct);

            if (resolveStadiumResult.IsError)
                return resolveStadiumResult.Errors;

            Stadium? stadium = resolveStadiumResult.Value;

            // Create new Stage if exists in request
            var resolveStageResult = await ResolveStage(request, ct);

            if (resolveStageResult.IsError)
                return resolveStageResult.Errors;

            Stage? stage = resolveStageResult.Value;

            // Create the Event
            var @event = Event.Create(
                name: request.Name,
                season: request.Season,
                status: Enum.Parse<EventStatus>(request.Status, ignoreCase: true),
                homeTeamId: homeTeam.Id,
                awayTeamId: awayTeam.Id,
                timeVenueUtc:request.TimeVenueUtc,
                dateVenueUtc: request.DateVenueUtc.Date,
                competitionId: competition.Id,
                stageId: stage?.Id,
                stadiumId: stadium?.Id,
                description:request.Description,
                resultId: null
            );

            await _eventRepository.AddAsync(@event, _unitOfWork.Transaction!, ct);

            // Create new Result if exists in request
            if (request.Result != null)
            {
                var result = Domain.Models.Result.Create(
                    periodScores: [],
                    matchIncidents: [],
                    eventId: @event.Id,
                    winnerId: request.Result.WinnerSide == TeamSide.Home ? homeTeam.Id : awayTeam.Id,
                    message: request.Result.Message
                ); 
                
                // Create PeriodScore models if any present in the request
                List<PeriodScore> periodScores = [];
                if (request.Result.PeriodScores.Any())
                {
                    foreach (var periodScoreCommand in request.Result.PeriodScores)
                    {
                        PeriodScore periodScore = PeriodScore.Create(
                            periodNumber: periodScoreCommand.PeriodNumber,
                            homeScore: periodScoreCommand.HomeScore,
                            awayScore: periodScoreCommand.AwayScore,
                            resultId: result.Id
                        );

                        periodScores.Add(periodScore);
                    }
                }

                // Create MatchIncident models if any present in the request
                List<MatchIncident> matchIncidents = [];
                if (request.Result.Incidents.Any())
                {
                    foreach (var matchIncidentCommand in request.Result.Incidents)
                    {
                        MatchIncident matchIncident = MatchIncident.Create(
                            matchMinute: matchIncidentCommand.MatchMinute,
                            type: Enum.Parse<IncidentType>(matchIncidentCommand.Type, ignoreCase: true),
                            teamId: matchIncidentCommand.Side == TeamSide.Home ? homeTeam.Id : awayTeam.Id,
                            resultId: result.Id
                        );

                        matchIncidents.Add(matchIncident);
                    }
                }

                result.AddScores(periodScores);
                result.AddIncidents(matchIncidents);

                await _resultRepository.AddAsync(result, _unitOfWork.Transaction!, ct);

                @event.Result = result;
            }

            // Hydate Event Model
            @event.HomeTeam = homeTeam;
            @event.AwayTeam = awayTeam;
            @event.Competition = competition;
            @event.Stadium = stadium;
            @event.Stage = stage;

            _unitOfWork.Commit();
            return @event;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while saving the event: {ex.Message}");
            if (_unitOfWork.Transaction != null) _unitOfWork.Rollback();
            return Error.Failure(
                code: "Event.CreationFailed",
                description: "An unexpected error occurred while saving the event."
            );
        }
    }

    
    // Creates new HomeTeam/AwayTeam if exists in request
    private async Task<ErrorOr<Team>> ResolveTeam(AddEventCommand request, TeamSide side, CancellationToken ct)
    {
        var existingTeamId = side == TeamSide.Home 
            ? request.ExistingHomeTeamId 
            : request.ExistingAwayTeamId;
        
        if (existingTeamId.HasValue)
        {
            var team = await _teamRepository.GetByIdAsync(existingTeamId.Value);
            return team == null 
                ? Error.NotFound(
                    code: $"{(side == TeamSide.Home ? "HomeTeam" : "AwayTeam")}.NotFound", 
                    description: $"Team with ID '{existingTeamId}' not found or doesn't exist.")
                : team;
        }

        var newTeamData = side == TeamSide.Home
            ? request.NewHomeTeam
            : request.NewAwayTeam;
        
        if (newTeamData != null)
        {
            var team = Team.Create(
                name: newTeamData.Name,
                officialName: newTeamData.OfficialName,
                abbreviation: newTeamData.Abbreviation,
                countryCode: newTeamData.CountryCode,
                sportId: newTeamData.SportId,
                stagePosition: newTeamData.StagePosition
            );
            
            await _teamRepository.AddAsync(team, _unitOfWork.Transaction!, ct);
            
            return team;
        }

        return Error.Validation(
            code: $"{(side == TeamSide.Home ? "HomeTeam" : "AwayTeam")}.Required",
            description: "New team data is missing.");
    }

    
    // Creates new Competition if exists in request
    private async Task<ErrorOr<Competition>> ResolveCompetition(AddEventCommand request, CancellationToken ct)
    {
        if (request.ExistingCompetitionId.HasValue)
        {
            var competition = await _competitionRepository.GetByIdAsync(request.ExistingCompetitionId.Value);
            return competition == null 
                ? Error.NotFound("Competition.NotFound", "Competition not found or doesn't exist.")
                : competition;
        }

        if (request.NewCompetition != null)
        {
            var competition = Competition.Create(
                name: request.NewCompetition.Name,
                sportId: request.NewCompetition.SportId
            );

            await _competitionRepository.AddAsync(competition, _unitOfWork.Transaction!, ct);

            return competition;
        }

        return Error.Validation("Competition.Required", "Competition data is missing.");
    }

    
    // Creates new Stadium if exists in request
    private async Task<ErrorOr<Stadium>> ResolveStadium(AddEventCommand request, CancellationToken ct)
    {
        if (request.ExistingStadiumId.HasValue)
        {
            var stadium = await _stadiumRepository.GetByIdAsync(request.ExistingStadiumId.Value);
            return stadium == null
                ? Error.NotFound("Stadium.NotFound", "Stadium not found or doesn't exist.")
                : stadium;
        }

        if (request.NewStadium != null)
        {
            var stadium = Stadium.Create(
                name: request.NewStadium.Name,
                countryCode: request.NewStadium.CountryCode
            );
            
            await _stadiumRepository.AddAsync(stadium, _unitOfWork.Transaction!, ct);

            return stadium;
        }

        return (Stadium)null!;
    }

    
    // Creates new Stage if exists in request
    private async Task<ErrorOr<Stage>> ResolveStage(AddEventCommand request, CancellationToken ct)
    {
        if (request.ExistingStageId.HasValue)
        {
            var stage = await _stageRepository.GetByIdAsync(request.ExistingStageId.Value);
            return stage == null
                ? Error.NotFound("Stage.NotFound", "Stage not found or doesn't exist.")
                : stage;
        }

        if (request.NewStage != null)
        {
            var stage = Stage.Create(
                name: request.NewStage.Name,
                ordering: request.NewStage.Ordering
            );

            await _stageRepository.AddAsync(stage, _unitOfWork.Transaction!, ct);

            return stage;
        }

        return (Stage)null!;
    }
}
