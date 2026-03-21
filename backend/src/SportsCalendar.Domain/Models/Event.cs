using SportsCalendar.Domain.Enums;

namespace SportsCalendar.Domain.Models;

public class Event
{
    // Core Properties
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public int Season { get; private set; }
    public EventStatus Status { get; private set; }
    public TimeSpan TimeVenueUtc { get; private set; }
    public DateTime DateVenueUtc { get; private set; }

    // DB Anchors
    public Guid HomeTeamId { get; init; }
    public Guid AwayTeamId { get; init; }
    public Guid? StadiumId { get; init; }
    public Guid? StageId { get; init; }
    public Guid CompetitionId { get; init; }
    public Guid? ResultId { get; private set; }

    // Navigation Properties
    public Team HomeTeam { get; set; } = null!;
    public Team AwayTeam { get; set; } = null!;
    public Stadium? Stadium { get; set; }
    public Stage? Stage { get; set; }
    public Competition Competition { get; set; } = null!;
    public Result? Result { get; set; }

    // Metadata
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; private set; }

    private Event() { }

    private Event(Guid id,
        string name,
        int season,
        Guid homeTeamId,
        Guid awayTeamId,
        TimeSpan timeVenueUtc,
        DateTime dateVenueUtc,
        Guid? stadiumId,
        Guid competitionId,
        Guid? resultId,
        EventStatus status = EventStatus.Scheduled,
        string? description = null,
        Guid? stageId = null)
    {
        Id = id;
        Name = name.Trim();
        Description = description?.Trim();
        Season = season;
        Status = status;
        TimeVenueUtc = timeVenueUtc;
        DateVenueUtc = dateVenueUtc;
        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;
        StadiumId = stadiumId;
        StageId = stageId;
        CompetitionId = competitionId;
        ResultId = resultId;

        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public static Event Create(string name,
        int season,
        EventStatus status,
        Guid homeTeamId,
        Guid awayTeamId,
        TimeSpan timeVenueUtc,
        DateTime dateVenueUtc,
        Guid? stadiumId,
        Guid competitionId,
        Guid? resultId,
        string? description = null,
        Guid? stageId = null)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Name required.");
        
        if (homeTeamId == Guid.Empty || awayTeamId == Guid.Empty) 
            throw new ArgumentException("Team IDs cannot be empty.");
        
        if (competitionId == Guid.Empty) 
            throw new ArgumentException("Competition ID required.");

        if (stadiumId.HasValue && stadiumId == Guid.Empty)
            throw new ArgumentException("Stadium ID required.");

        if (stageId.HasValue && stageId == Guid.Empty)
            throw new ArgumentException("Stage ID required.");

        if (resultId.HasValue && resultId == Guid.Empty)
            throw new ArgumentException("Result ID required.");

        return new Event(Guid.NewGuid(),
            name,
            season,
            homeTeamId,
            awayTeamId,
            timeVenueUtc,
            dateVenueUtc,
            stadiumId,
            competitionId,
            resultId,
            status,
            description,
            stageId);
    }
}