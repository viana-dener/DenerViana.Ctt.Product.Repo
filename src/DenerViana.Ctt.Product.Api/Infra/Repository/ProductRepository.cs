using DenerViana.Ctt.Product.Api.Domain.Interfaces;
using DenerViana.Ctt.Product.Api.Infra.Context;
using DenerViana.Ctt.Product.Api.Tools;
using MongoDB.Driver;

namespace DenerViana.Ctt.Product.Api.Infra.Repository;

public class ProductRepository(IMongoDbContext context) : IProductRepository
{
    #region Properties

    private readonly IMongoDbContext _context = context;

    #endregion

    #region Public Methods

    public async Task<IEnumerable<Domain.Entities.Product>> GetAllAsync()
    {
        return await _context.Products.Find(_ => true).ToListAsync();
    }
    public async Task<Domain.Entities.Product> GetByIdAsync(Guid id)
    {
        try
        {
            var filter = Builders<Domain.Entities.Product>.Filter.Eq(p => p.Id, id);
            var result = await _context.Products.Find(filter).FirstOrDefaultAsync();

            return result;
        }
        catch (Exception ex)
        {
            throw new DataException("Error!", ex, 500);
        }
    }
    public async Task<bool> ExistsAsync(Guid id)
    {
        var filter = Builders<Domain.Entities.Product>.Filter.Eq(p => p.Id, id);
        var product = await _context.Products.Find(filter).FirstOrDefaultAsync();
        return product != null;
    }
    public async Task<bool> ExistsAsync(string description)
    {
        var filter = Builders<Domain.Entities.Product>.Filter.Eq(p => p.Description, description);
        var product = await _context.Products.Find(filter).FirstOrDefaultAsync();
        return product != null;
    }
    public async Task<bool> AddAsync(Domain.Entities.Product product)
    {
        try
        {
            await _context.Products.InsertOneAsync(product);
            return true;
        }
        catch (MongoWriteException ex) when (ex.WriteError.Code == 11000) // Duplicated key error
        {
            throw new DataException("A product with the same ID already exists.", 409);
        }
        catch (Exception ex)
        {
            throw new DataException("Error while inserting the product.", ex, 500);
        }
    }

    #endregion
}
