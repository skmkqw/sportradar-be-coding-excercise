using System.Reflection;
using System.Text.Json.Serialization;
using Mapster;
using MapsterMapper;

namespace SportsCalendar.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddCors(options =>
        {
            options.AddPolicy(
                name: "AllowFrontend",
                configurePolicy: policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                }
            );
        });
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