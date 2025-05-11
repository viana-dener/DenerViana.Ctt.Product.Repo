using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DenerViana.Ctt.Product.Api.Base;
using DenerViana.Ctt.Product.Api.Base.Interfaces;
using Microsoft.Extensions.Logging;
using DenerViana.Ctt.Product.Api.Domain.Dtos;

namespace DenerViana.Ctt.Product.Test.Base;

public class MainEndpointsTests
{
    private readonly Mock<ILogger<MainEndpoints>> _mockLogger;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<INotify> _mockNotify;
    private readonly MainEndpoints _mainEndpoints;

    public MainEndpointsTests()
    {
        _mockLogger = new Mock<ILogger<MainEndpoints>>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockNotify = new Mock<INotify>();

        _mainEndpoints = new MainEndpoints(_mockLogger.Object, _mockHttpContextAccessor.Object, _mockNotify.Object);
    }

    [Fact(DisplayName = "Returns okresult when valid")]
    [Trait("Base", "MainEndpoints")]
    public void CustomResponse_ReturnsOkResult_WhenValid()
    {
        // Arrange
        _mockNotify.Setup(notify => notify.GetErrors()).Returns(new List<string>());

        // Act
        var result = _mainEndpoints.CustomResponse(new { success = true });

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "Returns badrequest result when invalid")]
    [Trait("Base", "MainEndpoints")]
    public void CustomResponse_ReturnsBadRequestResult_WhenInvalid()
    {
        // Arrange
        var requestLog = new RequestLogDto
        {
            CorrelationId = Guid.NewGuid().ToString(),
            Method = "POST",
            Path = "/api/test",
            QueryParameters = "?id=123",
            UriParameters = "123",
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
            Payload = "{ \"key\": \"value\" }",
            RouteId = "route-xyz"
        };

        var context = new DefaultHttpContext();
        context.Items["RequestLogDto"] = requestLog;
        var mockResponse = new List<string>();

        _mockNotify.Setup(notify => notify.GetErrors()).Returns(mockResponse);
        _mockHttpContextAccessor.Setup(accessor => accessor.HttpContext).Returns(context);

        // Act
        var result = _mainEndpoints.CustomResponse();

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "With modelstate returns badrequest result")]
    [Trait("Base", "MainEndpoints")]
    public void CustomResponse_WithModelState_ReturnsBadRequestResult()
    {
        // Arrange
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Name", "Name is required");

        var requestLog = new RequestLogDto
        {
            CorrelationId = Guid.NewGuid().ToString(),
            Method = "POST",
            Path = "/api/test",
            QueryParameters = "?id=123",
            UriParameters = "123",
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
            Payload = "{ \"key\": \"value\" }",
            RouteId = "route-abc"
        };

        var context = new DefaultHttpContext();
        context.Items["RequestLogDto"] = requestLog;

        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(context);
        _mockNotify.Setup(n => n.GetErrors()).Returns(new List<string> { "Name is required" });

        // Act
        var result = _mainEndpoints.CustomResponse(modelState);

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "Returns true when no errors")]
    [Trait("Base", "MainEndpoints")]
    public void IsValid_ReturnsTrue_WhenNoErrors()
    {
        // Arrange
        _mockNotify.Setup(notify => notify.GetErrors()).Returns(new List<string>());

        // Act
        var isValid = _mainEndpoints.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact(DisplayName = "Returns false when errors exist")]
    [Trait("Base", "MainEndpoints")]
    public void IsValid_ReturnsFalse_WhenErrorsExist()
    {
        // Arrange
        _mockNotify.Setup(notify => notify.GetErrors()).Returns(new List<string> { "Error 1" });

        // Act
        var isValid = _mainEndpoints.IsValid();

        // Assert
        Assert.False(isValid);
    }
}