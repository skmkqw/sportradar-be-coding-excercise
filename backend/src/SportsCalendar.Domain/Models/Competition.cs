namespace SportsCalendar.Domain.Models;

public class Competition
{
    public Guid Id { get; init; }

    public string Name { get; private set; }

    public string Slug { get; init; }

    public Guid SportId { get; init; }


    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; private set; }

    private Competition(Guid id, string name, string slug, Guid sportId)
    {
        Id = id;
        Name = name;
        Slug = slug;
        SportId = sportId;

        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public static Competition Create(string name, string slug, Guid sportId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Competition name is required.");

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Competition slug is required.");

        if (sportId == Guid.Empty)
            throw new ArgumentException("Sport id can't be empty");

        return new Competition(Guid.NewGuid(), name, slug, sportId);
    }
}