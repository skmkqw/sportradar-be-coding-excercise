using Mapster;
using SportsCalendar.Application.Common;
using SportsCalendar.Application.Events.Queries.GetEventById;
using SportsCalendar.Application.Events.Queries.GetEvents;
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

        config.NewConfig<GetEventsRequest, GetEventsQuery>();
        
        config.NewConfig<PagedResult<Event>, GetEventsResponse>()
                .Map(dest => dest.Events, src => src.Items)
                .Map(dest => dest.Metadata, src => src.PagingMetadata);
    }
}