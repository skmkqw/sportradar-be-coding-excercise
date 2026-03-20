namespace SportsCalendar.Domain.Models;

public class Country
{
    public string Code { get; init; }

    public string Name { get; private set; }

    public DateTime CreatedAtUtc { get; init; }

    private Country(string code, string name) 
    {
        Code = code;
        Name = name;


        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Country Create(string code, string name)
    {
        if (code.Length != 3) 
            throw new ArgumentException("Country code must be 3 characters.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Country name is required.");
        
        return new Country(code.ToUpper(), name);
    }
}