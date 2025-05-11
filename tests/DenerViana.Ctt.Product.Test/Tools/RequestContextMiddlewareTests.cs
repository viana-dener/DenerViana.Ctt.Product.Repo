using System.Text;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using DenerViana.Ctt.Product.Api.Tools;
using DenerViana.Ctt.Product.Api.Domain.Dtos;

public class RequestContextMiddlewareTests
{
    private readonly Mock<RequestDelegate> _mockNext;
    private readonly RequestContextMiddleware _middleware;

    public RequestContextMiddlewareTests()
    {
        _mockNext = new Mock<RequestDelegate>();
        _middleware = new RequestContextMiddleware(_mockNext.Object);
    }

    [Fact(DisplayName = "Should set RequestLogDto in HttpContext.Items")]
    [Trait("Tools", "RequestContextMiddleware")]
    public async Task InvokeAsync_ShouldSetRequestLogDtoInHttpContextItems()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/test-path";
        context.Request.Headers["x-correlation-id"] = "test-correlation-id";

        var bodyContent = "{\"key\":\"value\"}";
        var bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(bodyContent));
        context.Request.Body = bodyStream;

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        Assert.True(context.Items.ContainsKey("RequestLogDto"));

        var logDto = context.Items["RequestLogDto"] as RequestLogDto;
        Assert.NotNull(logDto);
        Assert.Equal("test-correlation-id", logDto.CorrelationId);
        Assert.Equal("GET", logDto.Method);
        Assert.Equal("/test-path", logDto.Path);
        Assert.Equal(bodyContent, logDto.Payload);
    }

    [Fact(DisplayName = "Should generate a new correlation ID if not provided")]
    [Trait("Tools", "RequestContextMiddleware")]
    public async Task InvokeAsync_ShouldGenerateCorrelationIdIfNotProvided()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = "/new-path";

        var bodyStream = new MemoryStream(Encoding.UTF8.GetBytes("{}"));
        context.Request.Body = bodyStream;

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        var logDto = context.Items["RequestLogDto"] as RequestLogDto;
        Assert.NotNull(logDto);
        Assert.NotNull(logDto.CorrelationId);
        Assert.True(Guid.TryParse(logDto.CorrelationId, out _));
    }

    [Fact(DisplayName = "Should capture query parameters correctly")]
    [Trait("Tools", "RequestContextMiddleware")]
    public async Task InvokeAsync_ShouldCaptureQueryParameters()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/products";
        context.Request.QueryString = new QueryString("?key1=value1&key2=value2");

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        var logDto = context.Items["RequestLogDto"] as RequestLogDto;
        Assert.NotNull(logDto);

        var queryParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(logDto.QueryParameters);
        Assert.NotNull(queryParams);
        Assert.Equal("value1", queryParams["key1"]);
        Assert.Equal("value2", queryParams["key2"]);
    }

    [Fact(DisplayName = "Should handle empty request body gracefully")]
    [Trait("Tools", "RequestContextMiddleware")]
    public async Task InvokeAsync_ShouldHandleEmptyRequestBody()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = "/empty-body";

        var bodyStream = new MemoryStream();
        context.Request.Body = bodyStream;

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        var logDto = context.Items["RequestLogDto"] as RequestLogDto;
        Assert.NotNull(logDto);
        Assert.Equal(string.Empty, logDto.Payload);
    }

    [Fact(DisplayName = "Should pass control to the next middleware")]
    [Trait("Tools", "RequestContextMiddleware")]
    public async Task InvokeAsync_ShouldCallNextMiddleware()
    {
        // Arrange
        var context = new DefaultHttpContext();

        _mockNext.Setup(next => next.Invoke(context)).Returns(Task.CompletedTask);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _mockNext.Verify(next => next.Invoke(context), Times.Once);
    }
}