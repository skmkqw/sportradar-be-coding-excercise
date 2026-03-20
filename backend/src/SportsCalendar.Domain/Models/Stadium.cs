namespace SportsCalendar.Domain.Models;

public class Stadium
{
    public Guid Id { get; init; }

    public string Name { get; private set; }

    public string CountryCode { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    private Stadium() { }

    private Stadium(Guid id, string name, string countryCode)
    {
        Id = id;
        Name = name.Trim();
        CountryCode = countryCode;

        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Stadium Create(string name, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Stadium name can't be empty.");

        countryCode = countryCode.Trim();
        if (countryCode.Length != 3)
            throw new ArgumentException("Country code must be 3 characters.");

        return new Stadium(Guid.NewGuid(), name, countryCode);
    }
}