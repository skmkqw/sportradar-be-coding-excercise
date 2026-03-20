namespace SportsCalendar.Domain.Models;

public class Stage
{
    public Guid Id { get; init; }

    public string Name { get; private set; }

    public int Ordering { get; private set; }

    public DateTime CreatedAtUtc { get; init; }

    private Stage(Guid id, string name, int ordering)
    {
        Id = id;
        Name = name;
        Ordering = ordering;

        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Stage Create(string name, int ordering)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Stage name is required.");

        if (ordering < 1)
            throw new ArgumentException("Ordering must be greater than 0.");

        return new Stage(Guid.NewGuid(), name, ordering);
    }
}