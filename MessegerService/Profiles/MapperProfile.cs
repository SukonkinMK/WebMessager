using AutoMapper;
using MessegerService.Models;

namespace MessegerService.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        { 
            CreateMap<MessageEntity, MessageModel>().ReverseMap();
        }
    }
}
