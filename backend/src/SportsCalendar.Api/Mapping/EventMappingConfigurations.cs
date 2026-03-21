using Mapster;
using SportsCalendar.Application.Events.Queries.GetEventById;

namespace SportsCalendar.Api.Mapping;

public class TeamMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, GetEventByIdQuery>()
            .Map(dest => dest.Id, src => src);
    }
}