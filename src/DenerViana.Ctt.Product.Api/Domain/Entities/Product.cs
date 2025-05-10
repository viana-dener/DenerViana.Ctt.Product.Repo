using DenerViana.Ctt.Product.Api.Base;
using MongoDB.Bson.Serialization.Attributes;

namespace DenerViana.Ctt.Product.Api.Domain.Entities;

/// <summary>
/// 
/// </summary>
public class Product : Entity
{
    #region Properties

    [BsonElement("stock")]
    public int Stock { get; private set; }

    [BsonElement("description")]
    public string Description { get; private set; }

    [BsonElement("categories")]
    public List<Guid> Categories { get; private set; } = new();

    [BsonElement("price")]
    public decimal Price { get; private set; }

    #endregion

    #region Builders

    private Product() { }
    
    [BsonConstructor]
    public Product(int stock, string description, List<Guid> categories, decimal price, string createdBy)
    {
        Stock = stock;
        Description = description;
        Categories = categories;
        Price = price;

        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    #endregion
}
