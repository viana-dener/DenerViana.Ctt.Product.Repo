using DenerViana.Ctt.Product.Api.Application.Interfaces;
using DenerViana.Ctt.Product.Api.Domain.Dtos;
using Serilog.Context;
using System.Net;

namespace DenerViana.Ctt.Product.Api.Application.Services;

public class LogInformation(ILogger<LogInformation> logger, IHttpContextAccessor httpContextAccessor) : ILogInformation
{
    private readonly ILogger<LogInformation> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void PublicherLog(string message)
    {
        var requestLog = _httpContextAccessor?.HttpContext?.Items["RequestLogDto"] as RequestLogDto;

        using (LogContext.PushProperty("CorrelationId", requestLog.CorrelationId))
        using (LogContext.PushProperty("StatusCode", (int)HttpStatusCode.OK))
        using (LogContext.PushProperty("Headers", requestLog.Headers))
        using (LogContext.PushProperty("Payload", requestLog.Payload))
        {
            _logger.LogInformation("Information(s) {Message}", message);
        }
    }
}
