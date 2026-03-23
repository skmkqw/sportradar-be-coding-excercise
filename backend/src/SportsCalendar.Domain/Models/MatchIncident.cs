using SportsCalendar.Domain.Enums;

namespace SportsCalendar.Domain.Models;

public class MatchIncident
{
    public Guid Id { get; init; }

    public Guid TeamId { get; init; }

    public IncidentType Type { get; init; }

    public int MatchMinute { get; private set; }

    public Guid ResultId { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    private MatchIncident() { }

    private MatchIncident(Guid id, Guid teamId, IncidentType type, int matchMinute, Guid resultId)
    {
        Id = id;
        TeamId = teamId;
        Type = type;
        MatchMinute = matchMinute;
        ResultId = resultId;

        CreatedAtUtc = DateTime.UtcNow;
    }

    public static MatchIncident Create(Guid teamId, IncidentType type, int matchMinute, Guid resultId)
    {
        if (teamId == Guid.Empty)
            throw new ArgumentException("Team id can't be empty.");

        if (resultId == Guid.Empty)
            throw new ArgumentException("Result id can't be empty.");

        if (matchMinute < 1)
            throw new ArgumentException("Match minute must be greater than 0.");

        return new MatchIncident(Guid.NewGuid(), teamId, type, matchMinute, resultId);
    }
}