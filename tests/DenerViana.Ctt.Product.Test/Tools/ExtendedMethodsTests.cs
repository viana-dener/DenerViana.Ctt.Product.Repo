using Microsoft.AspNetCore.Http;
using Moq;
using DenerViana.Ctt.Product.Api.Tools;

public class ExtendedMethodsTests
{
    [Fact(DisplayName = "Should extract only numbers from a string")]
    [Trait("Tools", "ExtendedMethods")]
    public void OnlyNumbers_ShouldExtractNumbers()
    {
        // Arrange
        var input = "abc123def456";
        var expected = "123456";

        // Act
        var result = input.OnlyNumbers();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact(DisplayName = "Should validate a valid email")]
    [Trait("Tools", "ExtendedMethods")]
    public void IsEmailValid_ShouldReturnTrue_ForValidEmail()
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var result = email.IsEmailValid();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Should invalidate an invalid email")]
    [Trait("Tools", "ExtendedMethods")]
    public void IsEmailValid_ShouldReturnFalse_ForInvalidEmail()
    {
        // Arrange
        var email = "invalid-email";

        // Act
        var result = email.IsEmailValid();

        // Assert
        Assert.False(result);
    }

    [Fact(DisplayName = "Should validate a valid tax number")]
    [Trait("Tools", "ExtendedMethods")]
    public void IsTaxNumberValid_ShouldReturnTrue_ForValidTaxNumber()
    {
        // Arrange
        var nif = "123456789";

        // Act
        var result = nif.IsTaxNumberValid();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Should invalidate an invalid tax number")]
    [Trait("Tools", "ExtendedMethods")]
    public void IsTaxNumberValid_ShouldReturnFalse_ForInvalidTaxNumber()
    {
        // Arrange
        var nif = "123";

        // Act
        var result = nif.IsTaxNumberValid();

        // Assert
        Assert.False(result);
    }

    [Fact(DisplayName = "Should generate a new correlation ID when not provided in headers")]
    [Trait("Tools", "ExtendedMethods")]
    public void GetCorrelationId_ShouldGenerateNewCorrelationId_WhenNotProvided()
    {
        // Arrange
        var mockAccessor = new Mock<IHttpContextAccessor>();
        mockAccessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext());

        // Act
        var result = ExtendedMethods.GetCorrelationId(mockAccessor.Object);

        // Assert
        Assert.NotNull(result);
        Assert.True(Guid.TryParse(result, out _));
    }

    [Fact(DisplayName = "Should return correlation ID from headers if provided")]
    [Trait("Tools", "ExtendedMethods")]
    public void GetCorrelationId_ShouldReturnCorrelationIdFromHeaders()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();
        var context = new DefaultHttpContext();
        context.Request.Headers["x-correlation-id"] = correlationId;

        var mockAccessor = new Mock<IHttpContextAccessor>();
        mockAccessor.Setup(a => a.HttpContext).Returns(context);

        // Act
        var result = ExtendedMethods.GetCorrelationId(mockAccessor.Object);

        // Assert
        Assert.Equal(correlationId, result);
    }

    [Fact(DisplayName = "Should validate headers and return errors for missing required headers")]
    [Trait("Tools", "ExtendedMethods")]
    public void ValidateHeaders_ShouldReturnErrors_ForMissingRequiredHeaders()
    {
        // Arrange
        var headers = new HeaderDictionary();
        headers["x-userid"] = "user123";

        var requiredHeaders = new Dictionary<string, bool>
        {
            { "x-userid", true },
            { "x-correlationid", true }
        };

        // Act
        var result = headers.ValidateHeaders(requiredHeaders);

        // Assert
        Assert.False(result.Result);
        Assert.Contains("x-correlationid", result.Errors.Keys);
    }

    [Fact(DisplayName = "Should validate headers successfully when all required headers are present")]
    [Trait("Tools", "ExtendedMethods")]
    public void ValidateHeaders_ShouldPass_ForAllRequiredHeadersPresent()
    {
        // Arrange
        var headers = new HeaderDictionary
        {
            { "x-userid", "user123" },
            { "x-correlationid", "correlation123" }
        };

        var requiredHeaders = new Dictionary<string, bool>
        {
            { "x-userid", true },
            { "x-correlationid", true }
        };

        // Act
        var result = headers.ValidateHeaders(requiredHeaders);

        // Assert
        Assert.True(result.Result);
    }

    [Fact(DisplayName = "Should validate a valid GUID")]
    [Trait("Tools", "ExtendedMethods")]
    public void IsGuid_ShouldReturnTrue_ForValidGuid()
    {
        // Arrange
        var input = Guid.NewGuid().ToString();

        // Act
        var result = input.IsGuid();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Should invalidate an invalid GUID")]
    [Trait("Tools", "ExtendedMethods")]
    public void IsGuid_ShouldReturnFalse_ForInvalidGuid()
    {
        // Arrange
        var input = "invalid-guid";

        // Act
        var result = input.IsGuid();

        // Assert
        Assert.False(result);
    }
}