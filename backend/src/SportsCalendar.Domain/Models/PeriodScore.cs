namespace SportsCalendar.Domain.Models;

public class PeriodScore
{
    public Guid Id { get; init; }

    public int PeriodNumber { get; init; }

    public int HomeScore { get; private set; }

    public int AwayScore { get; private set; }

    public Guid ResultId { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    private PeriodScore() { }

    private PeriodScore(Guid id, int periodNumber, int homeScore, int awayScore, Guid resultId)
    {
        Id = id;
        PeriodNumber = periodNumber;
        HomeScore = homeScore;
        AwayScore = awayScore;
        ResultId = resultId;

        CreatedAtUtc = DateTime.UtcNow;
    }

    public static PeriodScore Create(int periodNumber, int homeScore, int awayScore, Guid resultId)
    {
        if (periodNumber < 0)
            throw new ArgumentException("Period number can't be less than 0.");

        if (homeScore < 0)
            throw new ArgumentException("Home score can't be less than 0.");

        if (awayScore < 0)
            throw new ArgumentException("Away score can't be less than 0.");

        if (resultId == Guid.Empty)
            throw new ArgumentException("Result id can't be empty.");

        return new PeriodScore(Guid.NewGuid(), periodNumber, homeScore, awayScore, resultId);
    }
}