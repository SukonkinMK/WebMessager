using AutoMapper;
using UserService.Models;

namespace UserService.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserModel>()
                .ForMember("Email", opt => opt.MapFrom(src => src.Login))
                .ForMember("UserId", opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
