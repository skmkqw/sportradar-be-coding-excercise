using System.Data;
using Dapper;
using SportsCalendar.Domain.Models;
using SportsCalendar.Infrastructure.Extensions;

namespace SportsCalendar.Infrastructure.Repositories.Events;

public class EventRepository : IEventRepository
{
    private readonly IDbConnection _db;

    public EventRepository(IDbConnection db) => _db = db;

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var sql = "SELECT * FROM Events WHERE Id = @Id";

        return await _db.QueryFirstOrDefaultWithTokenAsync<Event>(sql, new { Id = id }, ct);
    }

    public async Task<IEnumerable<Event>> GetAllAsync(int page = 1,
        int pageSize = 10,
        string? sportSlug = null,
        DateTime? date = null,
        CancellationToken ct = default)
    {
        int offset = pageSize * (page - 1);
        var sql = @"
            SELECT e.*
            FROM Events e
            INNER JOIN Competitions c ON e.CompetitionId = c.Id 
            INNER JOIN Sports sp ON c.SportId = sp.Id
            WHERE (@SportSlug IS NULL OR sp.Slug = @SportSlug)
            AND (@Date IS NULL OR e.DateVenueUTC = @Date)
            ORDER BY e.DateVenueUTC, e.TimeVenueUTC 
            OFFSET @Offset FETCH @PageSize ROWS ONLY";

        return await _db.QueryWithTokenAsync<Event>(sql, new
        {
            Offset = offset,
            PageSize = pageSize,
            SportSlug = sportSlug,
            Date = date
        }, ct);
    }

    public async Task<Guid> AddAsync(Event @event, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO Events (Id, Name, Description, Season, Status, TimeVenueUTC, DateVenueUTC, GroupName, HomeTeamId, AwayTeamId, StadiumId, StageId, CompetitionId)
            VALUES (@Id, @Name, @Description, @Season, @Status, @TimeVenueUTC, @DateVenueUTC, @GroupName, @HomeTeamId, @AwayTeamId, @StadiumId, @StageId, @CompetitionId)";

        await _db.ExecuteAsync(sql, @event);
        return @event.Id;
    }
}

