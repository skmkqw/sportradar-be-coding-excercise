namespace SportsCalendar.Application.Common;

public class PagedResult<T>
{
    public List<T> Items { get; init; } = new List<T>();

    public Metadata PagingMetadata { get; init; } = null!;
    
    public record Metadata(int Page, int PageSize, int Total);

    public static PagedResult<T> Create(List<T> items, int page, int pageSize, int total)
    {
        return new PagedResult<T> { Items = items, PagingMetadata = new Metadata(page, pageSize, total) };
    }
}