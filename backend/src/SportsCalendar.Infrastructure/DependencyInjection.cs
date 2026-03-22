using System.Data;
using Dapper;
using DotNetEnv;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Infrastructure.Extensions;
using SportsCalendar.Infrastructure.Repositories.Events;

namespace SportsCalendar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Env.Load("../../../");

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")?.Trim();

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ApplicationException("Missing connection sting.");
        
        // DateOnly Type Handler
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        
        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

        // Repositories
        services.AddScoped<IEventRepository, EventRepository>();

        // Healthcheks
        services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("API is responsive"))
            .AddSqlServer(
                connectionString: connectionString!,
                name: "sqlserver",
                tags: ["db", "sql"]
            );

        return services;
    }
}