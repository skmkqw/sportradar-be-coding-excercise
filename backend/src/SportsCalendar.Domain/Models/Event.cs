using SportsCalendar.Domain.Enums;

namespace SportsCalendar.Domain.Models;

public class Event
{
    public Guid Id { get; init; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public int Season { get; private set; }

    public EventStatus Status { get; private set; }

    public Guid HomeTeamId { get; init; }

    public Guid AwayTeamId { get; init; }

    public Guid? StadiumId { get; init; }

    public Guid? StageId { get; init; }

    public Guid CompetitionId { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; private set; }

    private Event() { }

    private Event(Guid id,
        string name,
        int season,
        Guid homeTeamId,
        Guid awayTeamId,
        Guid? stadiumId,
        Guid competitionId,
        EventStatus status = EventStatus.Scheduled,
        string? description = null,
        Guid? stageId = null)
    {
        Id = id;
        Name = name.Trim();
        Description = description?.Trim();
        Season = season;
        Status = status;
        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;
        StadiumId = stadiumId;
        StageId = stageId;
        CompetitionId = competitionId;

        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public static Event Create(string name,
        int season,
        EventStatus status,
        Guid homeTeamId,
        Guid awayTeamId,
        Guid? stadiumId,
        Guid competitionId,
        string? description = null,
        Guid? stageId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Event name is required.");
        
        if (homeTeamId == Guid.Empty)
            throw new ArgumentException("Home team id can't be empty.");
        
        if (awayTeamId == Guid.Empty)
            throw new ArgumentException("Away team id can't be empty.");

        if (competitionId == Guid.Empty)
            throw new ArgumentException("Competition id can't be empty.");

        if (stadiumId.HasValue && stadiumId == Guid.Empty)
            throw new ArgumentException("Stadium id can't be empty.");

        if (stageId.HasValue && stageId == Guid.Empty)
            throw new ArgumentException("Stage id can't be empty.");

        return new Event(Guid.NewGuid(), name, season, homeTeamId, awayTeamId, stadiumId, competitionId,
           status, description, stageId);
    }
}