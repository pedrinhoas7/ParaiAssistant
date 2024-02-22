using AutoMapper;
using TheCats.Application.Entities;
using TheCats.Core.Entities;
using TheCats.Infrastructure.Entities;

namespace TheCats.API.Mappers;

public class AssistantMapperConfig : Profile
{
    public AssistantMapperConfig()
    {
        CreateMap<AssistantDTO, Assistant>().ReverseMap();
        CreateMap<Assistant, AssistantEntity>().ReverseMap();
    }
}
