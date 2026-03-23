using System.Data;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Domain.Models;
using SportsCalendar.Infrastructure.Extensions;

namespace SportsCalendar.Infrastructure.Repositories;

public class StageRepository : IStageRepository
{
    private readonly IDbConnection _dbConnection;

    public StageRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Stage?> GetByIdAsync(Guid id, IDbTransaction? transaction, CancellationToken ct = default)
    {
        const string sql = "SELECT * FROM Stages WHERE Id = @Id";

        return await _dbConnection.QueryFirstOrDefaultWithTokenAsync<Stage>(sql, transaction, new { Id = id }, ct);
    }

    public async Task AddAsync(Stage stage, IDbTransaction? transaction, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO Stages (Id, Name, Ordering)
            VALUES (@Id, @Name, @Ordering)";

        await _dbConnection.ExecuteWithTokenAsync(sql, stage, transaction, ct);
    }
}