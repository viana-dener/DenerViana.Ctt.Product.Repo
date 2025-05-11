using MongoDB.Driver;

namespace DenerViana.Ctt.Product.Api.Domain.Interfaces;

public interface IMongoDbContext
{
    IMongoCollection<Domain.Entities.Product> Products { get; }
}
