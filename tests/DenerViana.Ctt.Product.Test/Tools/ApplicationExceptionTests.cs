using System.Net;
using DenerViana.Ctt.Product.Api.Tools;

public class ApplicationExceptionTests
{
    [Fact(DisplayName = "Should set status code to bad request when no parameters provided")]
    [Trait("Tools", "ApplicationException")]
    public void Constructor_ShouldSetStatusCodeToBadRequest_WhenNoParametersProvided()
    {
        // Act
        var exception = new DenerViana.Ctt.Product.Api.Tools.ApplicationException();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal("Exception of type 'DenerViana.Ctt.Product.Api.Tools.ApplicationException' was thrown.", exception.Message);
    }

    [Fact(DisplayName = "Should set message and status code to bad request when only message is provided")]
    [Trait("Tools", "ApplicationException")]
    public void Constructor_ShouldSetMessageAndStatusCodeToBadRequest_WhenOnlyMessageIsProvided()
    {
        // Arrange
        var message = "Test error message";

        // Act
        var exception = new DenerViana.Ctt.Product.Api.Tools.ApplicationException(message);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal(message, exception.Message);
    }

    [Fact(DisplayName = "Should set message and custom status code when message and status code are provided")]
    [Trait("Tools", "ApplicationException")]
    public void Constructor_ShouldSetMessageAndCustomStatusCode_WhenMessageAndStatusCodeAreProvided()
    {
        // Arrange
        var message = "Test error message";
        var statusCode = (int)HttpStatusCode.NotFound;

        // Act
        var exception = new DenerViana.Ctt.Product.Api.Tools.ApplicationException(message, statusCode);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        Assert.Equal(message, exception.Message);
    }

    [Fact(DisplayName = "Should set message inner exception and status code when all parameters are provided")]
    [Trait("Tools", "ApplicationException")]
    public void Constructor_ShouldSetMessageInnerExceptionAndStatusCode_WhenAllParametersAreProvided()
    {
        // Arrange
        var message = "Test error message";
        var innerException = new Exception("Inner exception message");
        var statusCode = (int)HttpStatusCode.InternalServerError;

        // Act
        var exception = new DenerViana.Ctt.Product.Api.Tools.ApplicationException(message, innerException, statusCode);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }

    [Fact(DisplayName = "Should set default message and status code when only status code is provided")]
    [Trait("Tools", "ApplicationException")]
    public void Constructor_ShouldSetDefaultMessageAndStatusCode_WhenOnlyStatusCodeIsProvided()
    {
        // Arrange
        var statusCode = (int)HttpStatusCode.Forbidden;

        // Act
        var exception = new DenerViana.Ctt.Product.Api.Tools.ApplicationException(statusCode);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
        Assert.Equal($"An error occurred with the status {statusCode}", exception.Message);
    }
}