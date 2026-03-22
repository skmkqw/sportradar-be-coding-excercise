using System.Data;
using Dapper;
using SportsCalendar.Application.Common;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Infrastructure.Repositories.Events;

public class EventRepository : IEventRepository
{
    private readonly IDbConnection _db;

    public EventRepository(IDbConnection db) => _db = db;

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        const string sql = @"
            SELECT 
                e.*, 
                ht.*, 
                at.*, 
                st.*, 
                s.*, 
                c.*,
                r.* 
            FROM Events e
            JOIN Teams ht ON e.HomeTeamId = ht.Id
            JOIN Teams at ON e.AwayTeamId = at.Id
            LEFT JOIN Stadiums st ON e.StadiumId = st.Id
            LEFT JOIN Stages s ON e.StageId = s.Id
            JOIN Competitions c ON e.CompetitionId = c.Id
            LEFT JOIN Results r ON r.EventId = e.Id
            WHERE e.Id = @Id;

            SELECT ps.* FROM PeriodScores ps 
            JOIN Results r ON ps.ResultId = r.Id 
            WHERE r.EventId = @Id
            ORDER BY ps.PeriodNumber, ps.CreatedAtUtc;
            
            SELECT mi.* FROM MatchIncidents mi 
            JOIN Results r ON mi.ResultId = r.Id 
            WHERE r.EventId = @Id
            ORDER BY mi.MatchMinute;";

        await using var multi = await _db.QueryMultipleAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

        var eventEntity = multi.Read<Event, Team, Team, Stadium, Stage, Competition, Result, Event>(
            (ev, ht, at, st, s, cp, res) =>
            {
                ev.HomeTeam = ht; 
                ev.AwayTeam = at; 
                ev.Stadium = st;
                ev.Stage = s; 
                ev.Competition = cp; 
                ev.Result = res;
                return ev;
            }, splitOn: "Id,Id,Id,Id,Id,Id").FirstOrDefault();

        if (eventEntity?.Result != null)
        {
            var scores = await multi.ReadAsync<PeriodScore>();
            var incidents = await multi.ReadAsync<MatchIncident>();

            eventEntity.Result.PeriodScores.AddRange(scores);
            eventEntity.Result.MatchIncidents.AddRange(incidents);
        }

        return eventEntity;
    }

    public async Task<PagedResult<Event>> GetEventsAsync(int page,
        int pageSize,
        Guid? sportId,
        DateOnly? start,
        DateOnly? end,
        CancellationToken ct)
    {
        int offset = (page - 1) * pageSize;

        var sql = @"
            DECLARE @Filtered TABLE (Id UNIQUEIDENTIFIER);

            INSERT INTO @Filtered (Id)
            SELECT e.Id
            FROM Events e
            INNER JOIN Competitions c ON e.CompetitionId = c.Id 
            INNER JOIN Sports sp ON c.SportId = sp.Id
            WHERE (@SportId IS NULL OR sp.Id = @SportId)
            AND (@Start IS NULL OR e.DateVenueUTC >= @Start)
            AND (@End IS NULL OR e.DateVenueUTC <= @End);

            SELECT COUNT(*) FROM @Filtered;

            SELECT e.*
            FROM Events e
            INNER JOIN @Filtered f ON e.Id = f.Id
            ORDER BY e.DateVenueUTC, e.TimeVenueUTC 
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        using var multi = await _db.QueryMultipleAsync(new CommandDefinition(sql,
            new
            {
                SportId = sportId,
                Start = start,
                End = end,
                Offset = offset,
                PageSize = pageSize
            },
            cancellationToken: ct));

        var total = await multi.ReadFirstAsync<int>();
        var events = (await multi.ReadAsync<Event>()).ToList();

        return PagedResult<Event>.Create(events, page, pageSize, total);
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

