using DenerViana.Ctt.Product.Api.Domain.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace DenerViana.Ctt.Product.Api.Tools;

public class RequestContextMiddleware
{
    private readonly RequestDelegate _next;

    public RequestContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering(); // Permite re-leitura do body

        var correlationId = context.Request.Headers.TryGetValue("x-correlation-id", out var cid)
            ? cid.FirstOrDefault() ?? Guid.NewGuid().ToString()
            : Guid.NewGuid().ToString();

        var uriParams = context.GetRouteData()?.Values
            .ToDictionary(k => k.Key, v => v.Value?.ToString() ?? string.Empty);

        var queryParams = context.Request.Query
            .ToDictionary(k => k.Key, v => v.Value.ToString());

        var requestLogDto = new RequestLogDto
        {
            CorrelationId = correlationId,
            Method = context.Request.Method,
            Path = context.Request.Path,
            QueryParameters = queryParams.Any()
                ? JsonConvert.SerializeObject(queryParams)
                : string.Empty,
            UriParameters = uriParams != null && uriParams.Any()
                ? JsonConvert.SerializeObject(uriParams)
                : string.Empty,
            Headers = context.Request.Headers.ToDictionary(k => k.Key, v => v.Value.ToString()),
            RouteId = context.GetRouteData()?.Values["id"]?.ToString(),
            Timestamp = DateTime.UtcNow,
            Payload = await ReadBody(context.Request)
        };

        context.Items["RequestLogDto"] = requestLogDto;

        await _next(context);
    }

    private static async Task<string> ReadBody(HttpRequest request)
    {
        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }
}
