using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using DenerViana.Ctt.Product.Api.Tools;
using DenerViana.Ctt.Product.Api.Domain.Dtos;

public class ExceptionMiddlewareTests
{
    private readonly Mock<RequestDelegate> _mockNext;
    private readonly Mock<ILogger<ExceptionMiddleware>> _mockLogger;
    private readonly ExceptionMiddleware _middleware;

    public ExceptionMiddlewareTests()
    {
        _mockNext = new Mock<RequestDelegate>();
        _mockLogger = new Mock<ILogger<ExceptionMiddleware>>();
        _middleware = new ExceptionMiddleware(_mockNext.Object, _mockLogger.Object);
    }

    [Fact(DisplayName = "Should handle application exception")]
    [Trait("Tools", "ExceptionMiddleware")]
    public async Task InvokeAsync_ShouldHandleApplicationException()
    {
        // Arrange
        _mockNext.Setup(next => next(It.IsAny<HttpContext>()))
            .Throws(new DenerViana.Ctt.Product.Api.Tools.ApplicationException("Test Application Exception"));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        context.Items["RequestLogDto"] = new RequestLogDto
        {
            CorrelationId = Guid.NewGuid().ToString(),
            Headers = new Dictionary<string, string>(),
            Payload = "{}"
        };

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Contains("A requisição está malformada ou contém dados inválidos.", responseBody);
    }

    [Fact(DisplayName = "Should handle data exception")]
    [Trait("Tools", "ExceptionMiddleware")]
    public async Task InvokeAsync_ShouldHandleDataException()
    {
        // Arrange
        _mockNext.Setup(next => next(It.IsAny<HttpContext>()))
            .Throws(new DataException("Test Data Exception", (int)HttpStatusCode.InternalServerError));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        context.Items["RequestLogDto"] = new RequestLogDto
        {
            CorrelationId = Guid.NewGuid().ToString(),
            Headers = new Dictionary<string, string>(),
            Payload = "{}"
        };

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Contains("Ocorreu um erro inesperado no servidor", responseBody);
    }

    [Fact(DisplayName = "Should handle domain exception")]
    [Trait("Tools", "ExceptionMiddleware")]
    public async Task InvokeAsync_ShouldHandleDomainException()
    {
        // Arrange
        _mockNext.Setup(next => next(It.IsAny<HttpContext>()))
            .Throws(new DomainException("Test Domain Exception", (int)HttpStatusCode.Forbidden));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        context.Items["RequestLogDto"] = new RequestLogDto
        {
            CorrelationId = Guid.NewGuid().ToString(),
            Headers = new Dictionary<string, string>(),
            Payload = "{}"
        };

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
        Assert.Contains("Você não tem permissão para acessar este recurso.", responseBody);
    }

    [Fact(DisplayName = "Should handle generic exception as internal servererror")]
    [Trait("Tools", "ExceptionMiddleware")]
    public async Task InvokeAsync_ShouldHandleGenericExceptionAsInternalServerError()
    {
        // Arrange
        _mockNext.Setup(next => next(It.IsAny<HttpContext>()))
            .Throws(new Exception("Test Generic Exception"));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        context.Items["RequestLogDto"] = new RequestLogDto
        {
            CorrelationId = Guid.NewGuid().ToString(),
            Headers = new Dictionary<string, string>(),
            Payload = "{}"
        };

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Contains("Ocorreu um erro inesperado no servidor", responseBody);
    }
}