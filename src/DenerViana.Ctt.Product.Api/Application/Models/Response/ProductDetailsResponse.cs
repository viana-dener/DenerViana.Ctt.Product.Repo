namespace DenerViana.Ctt.Product.Api.Application.Models.Response;

public class ProductDetailsResponse
{
    public string Id { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; }
    public List<Guid> Categories { get; set; }
    public decimal Price { get; set; }
}
