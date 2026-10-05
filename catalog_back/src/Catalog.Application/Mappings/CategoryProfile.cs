
using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings;

public class CategoryProfile:Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}