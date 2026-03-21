using Mapster;
using SportsCalendar.Application.Events.Queries.GetEventById;
using SportsCalendar.Contracts.Events;
using SportsCalendar.Domain.Models;

namespace SportsCalendar.Api.Mapping;

public class TeamMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, GetEventByIdQuery>()
            .Map(dest => dest.Id, src => src);

        config.NewConfig<Event, GetEventResponse>()
            .Map(dest => dest.Status, src => src.Status.ToString().ToLower())
            .Map(dest => dest.DateVenueUtc, src => DateOnly.FromDateTime(src.DateVenueUtc));

        config.NewConfig<MatchIncident, MatchIncidentResponse>()
            .Map(dest => dest.Type, src => src.Type.ToString().ToLower());

        config.NewConfig<Event, GetFullEventResponse>()
            .Map(dest => dest.Status, src => src.Status.ToString().ToLower())
            .Map(dest => dest.DateVenueUtc, src => DateOnly.FromDateTime(src.DateVenueUtc));
    }
}