using AutoMapper;
using TheCats.Core.Entities;
using TheCats.Infrastructure.Entities;

namespace TheCats.API.Mappers;

public class ConversationMapperConfig : Profile
{
    public ConversationMapperConfig()
    {
        CreateMap<Conversation, ConversationEntity>().ReverseMap();
    }
}
