using AutoMapper;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Models;

namespace VShop.ProductApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category,CategoryDTO>().ReverseMap();
            CreateMap<Product,ProductDTO>().ForMember(x=> x.CategoryName,opt=> opt.MapFrom(src => src.Category.Name));
        }
    }
}
