using SportsCalendar.Domain.Enums;

namespace SportsCalendar.Domain.Models;

public class Result
{
    public Guid Id { get; init; }

    public List<PeriodScore> PeriodScores { get; } = new List<PeriodScore>();

    public List<MatchIncident> MatchIncidents { get; } = new List<MatchIncident>();

    public int HomeScore => PeriodScores.Sum(s => s.HomeScore);

    public int AwayScore => PeriodScores.Sum(s => s.AwayScore);

    public Guid? WinnerId { get; private set; }

    public string? Message { get; private set; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; private set; }

    private Result() { }

    private Result(Guid id,
        List<PeriodScore>? periodScores,
        List<MatchIncident>? matchIncidents,
        Guid? winnerId = null,
        string? message = null)
    {
        Id = id;
        PeriodScores = periodScores ?? new List<PeriodScore>();
        MatchIncidents = matchIncidents ?? new List<MatchIncident>();
        WinnerId = winnerId;
        Message = message?.Trim();

        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public static Result Create(List<PeriodScore> periodScores,
        List<MatchIncident>? matchIncidents,
        Guid? winnerId = null,
        string? message = null)
    {        
        if (winnerId == Guid.Empty)
            throw new ArgumentException("Winner id can't be empty.");

        return new Result(Guid.NewGuid(), periodScores, matchIncidents, winnerId, message);
    }

    public void AddScore(int home, int away)
    {
        PeriodScores.Add(PeriodScore.Create(PeriodScores.Count + 1, home, away));
    }

    public void AddIncident(Guid teamId, IncidentType type, int matchMinute)
    {
        MatchIncidents.Add(MatchIncident.Create(teamId, type, matchMinute));
    }
}