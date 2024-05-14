using AutoMapper;
using projekatKVA.DTOs;
using projekatKVA.Models;

namespace projekatKVA.Configurations
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig() 
        {
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<ItemDTO, Item>().ReverseMap();
        }
    }
}
