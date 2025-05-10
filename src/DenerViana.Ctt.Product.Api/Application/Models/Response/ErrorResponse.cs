namespace DenerViana.Ctt.Product.Api.Application.Models.Response;

public class ErrorResponse
{
    public string[] Errors { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
