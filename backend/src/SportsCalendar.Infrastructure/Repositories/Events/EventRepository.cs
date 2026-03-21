using System.Data;
using Dapper;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;
using SportsCalendar.Infrastructure.Extensions;

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

