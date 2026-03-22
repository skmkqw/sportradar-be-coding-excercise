namespace SportsCalendar.Contracts.Events;

public record GetEventsRequest(
    int Page = 1,
    int PageSize = 10, 
    Guid? SportId = null, 
    DateOnly? StartDate = null, 
    DateOnly? EndDate = null
);