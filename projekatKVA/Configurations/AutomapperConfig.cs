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
            CreateMap<CartDTO, Cart>().ReverseMap();    
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<WishDTO, Wish>().ReverseMap();
            CreateMap<OrderItemDTO, OrderItem>().ReverseMap();
        }
    }
}
