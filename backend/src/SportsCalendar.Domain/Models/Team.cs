namespace SportsCalendar.Domain.Models;

public class Team
{
    public Guid Id { get; init; }

    public string Name { get; private set; }

    public string OfficialName { get; private set; }

    public string Slug { get; init; }

    public string Abbreviation { get; private set; }

    public string CountryCode { get; private set; }

    public Guid SportId { get; private set; }

    public int? StagePosition { get; private set; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; private set; }

    private Team() { }

    private Team(Guid id,
        string name,
        string officialName,
        string slug,
        string abbreviation,
        string countryCode,
        Guid sportId,
        int? stagePosition)
    {
        Id = id;
        Name = name.Trim();
        OfficialName = officialName.Trim();
        Slug = slug.Trim();
        Abbreviation = abbreviation;
        CountryCode = countryCode;
        SportId = sportId;
        StagePosition = stagePosition;


        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public static Team Create(string name,
        string officialName,
        string slug,
        string abbreviation,
        string countryCode,
        Guid sportId,
        int? stagePosition = null)
    {
        if (stagePosition.HasValue && stagePosition < 1)
            throw new ArgumentException("Stage position must be at least 1.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Team name is required.");

        if (string.IsNullOrWhiteSpace(officialName))
            throw new ArgumentException("Team official name is required.");

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Team slug is required.");

        if (string.IsNullOrWhiteSpace(abbreviation))
            throw new ArgumentException("Team abbreviation is required.");

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Team country code can't be empty.");

        if (sportId == Guid.Empty)
            throw new ArgumentException("Sport id can't be empty");


        return new Team(Guid.NewGuid(),
            name,
            officialName,
            slug,
            abbreviation,
            countryCode,
            sportId,
            stagePosition
        );
    }
}