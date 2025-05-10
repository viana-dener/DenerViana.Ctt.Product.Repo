namespace DenerViana.Ctt.Product.Api.Application.Models.Request;

public class ProductRequest
{
    public int Stock { get; set; }
    public string Description { get; set; }
    public List<Guid> Categories { get; set; } = new();
    public decimal Price { get; set; }
}
