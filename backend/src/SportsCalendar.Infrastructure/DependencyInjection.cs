using System.Data;
using Dapper;
using DotNetEnv;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SportsCalendar.Application.Interfaces;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Infrastructure.Extensions;
using SportsCalendar.Infrastructure.Repositories;

namespace SportsCalendar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        Env.Load("../../../");

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")?.Trim();

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ApplicationException("Missing connection sting.");
        
        // DateOnly Type Handler
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        
        // DbConnection
        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IResultRepository, ResultRepository>();
        services.AddScoped<ICompetitionRepository, CompetitionRepository>();
        services.AddScoped<IStadiumRepository, StadiumRepository>();
        services.AddScoped<IStageRepository, StageRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<ISportRepository, SportRepository>();

        // Healthcheks
        services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("API is responsive"))
            .AddSqlServer(
                connectionString: connectionString,
                name: "sqlserver",
                tags: ["db", "sql"]
            );

        return services;
    }
}