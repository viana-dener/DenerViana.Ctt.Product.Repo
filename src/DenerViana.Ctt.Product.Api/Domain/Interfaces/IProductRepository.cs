namespace DenerViana.Ctt.Product.Api.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Entities.Product>> GetAllAsync();
    Task<Entities.Product> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(string description);
    Task<bool> AddAsync(Entities.Product product);
}