using AutoMapper;
using DenerViana.Ctt.Product.Api.Application.Models.Request;
using DenerViana.Ctt.Product.Api.Application.Models.Response;


namespace DenerViana.Ctt.Product.Api.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Product, ProductResponse>().ReverseMap();
        CreateMap<Domain.Entities.Product, ProductRequest>().ReverseMap();
        CreateMap<Domain.Entities.Product, ProductDetailsResponse>().ReverseMap();
    }
}
