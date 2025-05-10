using DenerViana.Ctt.Product.Api.Domain.Interfaces;
using MongoDB.Driver;

namespace DenerViana.Ctt.Product.Api.Domain.Services;

public class ProductServices(IProductRepository repository) : IProductServices
{
    #region Properties

    private readonly IProductRepository _repository = repository;

    #endregion

    #region Public Methods

    public async Task<IEnumerable<Entities.Product>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
    public async Task<Entities.Product> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _repository.ExistsAsync(id);
    }
    public async Task<bool> ExistsAsync(string description)
    {
        return await _repository.ExistsAsync(description);
    }
    public async Task<bool> AddAsync(Entities.Product product)
    {
        return await _repository.AddAsync(product);
    }

    #endregion
}
