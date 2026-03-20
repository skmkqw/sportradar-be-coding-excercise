namespace SportsCalendar.Domain.Models;

public class Sport
{
    public Guid Id { get; init; }

    public string Name { get; private set; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; private set; }

    private Sport() { }

    private Sport(Guid id, string name)
    {
        Id = id;
        Name = name.Trim();

        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public static Sport Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Sport name is required.");
        
        return new Sport(Guid.NewGuid(), name);
    }
}