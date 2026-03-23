namespace SportsCalendar.Domain.Models;

public class Competition
{
    public Guid Id { get; init; }

    public string Name { get; private set; }

    public string Slug { get; init; }

    public Guid SportId { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; private set; }

    private Competition() { }

    private Competition(Guid id, string name, Guid sportId)
    {
        Id = id;
        Name = name.Trim();
        Slug = GenerateSlug(Name);
        SportId = sportId;

        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public static Competition Create(string name, Guid sportId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Competition name is required.");

        if (sportId == Guid.Empty)
            throw new ArgumentException("Sport id can't be empty");

        return new Competition(Guid.NewGuid(), name, sportId);
    }

    private static string GenerateSlug(string name)
        => name
            .Replace(' ', '_')
            .ToLower();
    }