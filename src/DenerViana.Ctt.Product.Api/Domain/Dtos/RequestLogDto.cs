namespace DenerViana.Ctt.Product.Api.Domain.Dtos;

public class RequestLogDto
{
    public string CorrelationId { get; set; }
    public string Method { get; set; }
    public string Path { get; set; }
    public string QueryParameters { get; set; }
    public string UriParameters { get; set; }
    public IDictionary<string, string> Headers { get; set; }
    public string Payload { get; set; }
    public string RouteId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
