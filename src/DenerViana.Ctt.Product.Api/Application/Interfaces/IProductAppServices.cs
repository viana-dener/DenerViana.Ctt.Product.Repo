using DenerViana.Ctt.Product.Api.Application.Models.Request;
using DenerViana.Ctt.Product.Api.Application.Models.Response;
using DenerViana.Ctt.Product.Api.Domain.Dtos;

namespace DenerViana.Ctt.Product.Api.Application.Interfaces;

public interface IProductAppServices
{
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<ProductDetailsResponse> GetByIdAsync(Guid id);

    Task<bool> AddAsync(ProductRequest request, UserInfoDto userInfo);
}
