namespace SportsCalendar.Domain.Models;

public class Result
{
    public Guid Id { get; init; }

    public List<PeriodScore> PeriodScores { get; }

    public int HomeScore => PeriodScores.Sum(s => s.HomeScore);

    public int AwayScore => PeriodScores.Sum(s => s.AwayScore);

    public Guid? WinnerId { get; private set; }

    public string? Message { get; private set; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; private set; }

    private Result() { }

    private Result(Guid id, List<PeriodScore>? periodScores, Guid? winnerId = null, string? message = null)
    {
        Id = id;
        PeriodScores = periodScores ?? new List<PeriodScore>();
        WinnerId = winnerId;
        Message = message?.Trim();

        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public static Result Create(List<PeriodScore> periodScores, Guid? winnerId = null, string? message = null)
    {        
        if (winnerId == Guid.Empty)
            throw new ArgumentException("Winner id can't be empty.");

        return new Result(Guid.NewGuid(), periodScores, winnerId, message);
    }

    public void AddScore(int home, int away)
    {
        PeriodScores.Add(PeriodScore.Create(PeriodScores.Count + 1, home, away));
    }
}