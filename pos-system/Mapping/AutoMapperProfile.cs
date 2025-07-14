using AutoMapper;
using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TblProductCategory, ProductCategoryDTO>().ForMember(dest => dest.TotalProducts, opt => opt.MapFrom(src => src.TblProducts.Count()));
            CreateMap<TblProduct, ProductDTO>();
            CreateMap<TblOrder, OrderDTO>();
            CreateMap<TblOrderItem, OrderItemDTO>();
            CreateMap<TblProductVariant, ProductVariantDTO>();
        }
    }
}
