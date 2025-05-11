using AutoMapper;
using DenerViana.Ctt.Product.Api.Application.Interfaces;
using DenerViana.Ctt.Product.Api.Application.Models.Request;
using DenerViana.Ctt.Product.Api.Application.Models.Response;
using DenerViana.Ctt.Product.Api.Base.Interfaces;
using DenerViana.Ctt.Product.Api.Domain.Dtos;
using DenerViana.Ctt.Product.Api.Domain.Entities;
using DenerViana.Ctt.Product.Api.Domain.Interfaces;

namespace DenerViana.Ctt.Product.Api.Application.Services;

public class ProductAppServices(ILogInformation logInformation, INotify notify, IMapper mapper, IProductServices services) : IProductAppServices
{
    #region Properties

    private readonly ILogInformation _logInformation = logInformation;
    private readonly INotify _notify = notify;
    private readonly IMapper _mapper = mapper;
    private readonly IProductServices _services = services;

    #endregion

    #region Public Methods

    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<ProductResponse>>(await _services.GetAllAsync());
    }

    public async Task<ProductDetailsResponse> GetByIdAsync(Guid id)
    {
        return _mapper.Map<ProductDetailsResponse>(await _services.GetByIdAsync(id));
    }

    public async Task<bool> AddAsync(ProductRequest request, UserInfoDto userInfo)
    {
        if (await _services.ExistsAsync(request.Description))
        {
            _notify.AddError("Product already exists", 409);
            return false;
        }

        var product = new Domain.Entities.Product(request.Stock, request.Description, request.Categories, request.Price, userInfo.UserId);

        return await _services.AddAsync(product);
    }

    #endregion
}
