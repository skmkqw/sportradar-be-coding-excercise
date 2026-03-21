using System.Reflection;
using Mapster;
using MapsterMapper;

namespace SportsCalendar.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMappers();
        services.AddSignalR();

        services.AddMappers();

        return services;
    }

    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}