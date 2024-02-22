using AutoMapper;
using TheCats.Application.Entities;
using TheCats.Core.Entities;
using TheCats.Infrastructure.Entities;

namespace TheCats.Exam.Mappers
{
    public class UserMapperConfig : Profile
    {
        public UserMapperConfig()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserEntity>().ReverseMap();
        }
    }
}
