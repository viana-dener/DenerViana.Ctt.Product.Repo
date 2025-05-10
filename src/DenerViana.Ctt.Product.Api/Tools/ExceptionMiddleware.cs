using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Serilog.Context;
using System.Net;
using DenerViana.Ctt.Product.Api.Application.Models.Response;
using DenerViana.Ctt.Product.Api.Domain.Dtos;

namespace DenerViana.Ctt.Product.Api.Tools;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ApplicationException ex)
        {
            await HandleExceptionAsync(httpContext, ex, ex.StatusCode);
        }
        catch (DomainException ex)
        {
            await HandleExceptionAsync(httpContext, ex, ex.StatusCode);
        }
        catch (DataException ex)
        {
            await HandleExceptionAsync(httpContext, ex, ex.StatusCode);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Conflict);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError);
        }
    }

    private static async Task CreateErrorMessage(HttpContext httpContext, string error)
    {
        var errorResponse = new ErrorResponse
        {
            Errors = new[] { error },
        };

        httpContext.Response.ContentType = "application/json";

        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var json = JsonConvert.SerializeObject(errorResponse, settings);
        await httpContext.Response.WriteAsync(json);
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex, HttpStatusCode statusCode)
    {
        LogRequestException(httpContext, statusCode, ex);
        httpContext.Response.StatusCode = (int)statusCode;

        var userMessage = statusCode switch
        {
            HttpStatusCode.Conflict => "Já existe um recurso com os mesmos dados. Verifique se há duplicidade.",
            HttpStatusCode.BadRequest => "A requisição está malformada ou contém dados inválidos.",
            HttpStatusCode.Forbidden => "Você não tem permissão para acessar este recurso.",
            HttpStatusCode.NotFound => "O recurso solicitado não foi encontrado.",
            HttpStatusCode.Gone => "Este recurso não está mais disponível e foi permanentemente removido.",
            HttpStatusCode.InternalServerError => "Ocorreu um erro inesperado no servidor. Nossa equipe foi notificada.",
            _ => ex.Message
        };

        await CreateErrorMessage(httpContext, userMessage);
    }

    private void LogRequestException(HttpContext httpContext, HttpStatusCode statusCode, Exception ex)
    {
        var requestLog = httpContext?.Items["RequestLogDto"] as RequestLogDto;

        using (LogContext.PushProperty("CorrelationId", requestLog.CorrelationId))
        using (LogContext.PushProperty("StatusCode", (int)statusCode))
        using (LogContext.PushProperty("Headers", requestLog.Headers))
        using (LogContext.PushProperty("Payload", requestLog.Payload))
        {
            _logger.LogError(ex, "Error(s) {Message}", ex.Message);
        }
    }
}
