
using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
    }
}