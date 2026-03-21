using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SportsCalendar.Application.Interfaces.Repositories;
using SportsCalendar.Infrastructure.Repositories.Events;

namespace SportsCalendar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

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