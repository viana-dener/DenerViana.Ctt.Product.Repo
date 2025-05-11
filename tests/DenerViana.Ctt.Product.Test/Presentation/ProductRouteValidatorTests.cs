using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using DenerViana.Ctt.Product.Api.Presentation.Validations;
using DenerViana.Ctt.Product.Api.Application.Models.Request;

public class ProductRouteValidatorTests
{
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly ProductRouteValidator _validator;

    public ProductRouteValidatorTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _validator = new ProductRouteValidator(_mockHttpContextAccessor.Object);
    }

    [Fact(DisplayName = "Should pass when valid model")]
    [Trait("Presentation", "ProductRouteValidator")]
    public void ValidateFields_ShouldPass_WhenValidModel()
    {
        // Arrange
        var request = new ProductRequest
        {
            Description = "Valid Product",
            Stock = 10,
            Price = 100.50m,
            Categories = new List<Guid> { Guid.NewGuid() }
        };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should fail when description is empty")]
    [Trait("Presentation", "ProductRouteValidator")]
    public void ValidateFields_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        var request = new ProductRequest
        {
            Description = "",
            Stock = 10,
            Price = 100.50m,
            Categories = new List<Guid> { Guid.NewGuid() }
        };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Description)
              .WithErrorMessage("The field description is required.");
    }

    [Fact(DisplayName = "Should fail when stock is negative")]
    [Trait("Presentation", "ProductRouteValidator")]
    public void ValidateFields_ShouldFail_WhenStockIsNegative()
    {
        // Arrange
        var request = new ProductRequest
        {
            Description = "Valid Product",
            Stock = -5,
            Price = 100.50m,
            Categories = new List<Guid> { Guid.NewGuid() }
        };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Stock)
              .WithErrorMessage("The stock must be greater than or equal to 0.");
    }

    [Fact(DisplayName = "Should fail when price is zero or negative")]
    [Trait("Presentation", "ProductRouteValidator")]
    public void ValidateFields_ShouldFail_WhenPriceIsZeroOrNegative()
    {
        // Arrange
        var request = new ProductRequest
        {
            Description = "Valid Product",
            Stock = 10,
            Price = 0,
            Categories = new List<Guid> { Guid.NewGuid() }
        };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Price)
              .WithErrorMessage("The price must be greater than 0.");
    }

    [Fact(DisplayName = "Should fail when categories are empty")]
    [Trait("Presentation", "ProductRouteValidator")]
    public void ValidateFields_ShouldFail_WhenCategoriesAreEmpty()
    {
        // Arrange
        var request = new ProductRequest
        {
            Description = "Valid Product",
            Stock = 10,
            Price = 100.50m,
            Categories = new List<Guid>()
        };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Categories)
              .WithErrorMessage("At least one category is required.");
    }

    [Fact(DisplayName = "Should fail when id is invalid")]
    [Trait("Presentation", "ProductRouteValidator")]
    public void ValidateRoute_ShouldFail_WhenIdIsInvalid()
    {
        // Arrange
        var routeData = new RouteData();
        routeData.Values.Add("id", "invalid-guid");

        var httpContext = new DefaultHttpContext();
        httpContext.Features.Set<IRoutingFeature>(new RoutingFeature { RouteData = routeData });
        httpContext.Request.Method = HttpMethod.Put.Method;

        _mockHttpContextAccessor.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        var validator = new ProductRouteValidator(_mockHttpContextAccessor.Object);

        // Act & Assert
        var result = validator.TestValidate(new ProductRequest());
        result.ShouldHaveValidationErrorFor("id")
              .WithErrorMessage("The value entered in the field is not a valid Guid.");
    }

    [Fact(DisplayName = "Should pass when id is valid")]
    [Trait("Presentation", "ProductRouteValidator")]
    public void ValidateRoute_ShouldPass_WhenIdIsValid()
    {
        // Arrange
        var validId = Guid.NewGuid().ToString();
        var routeData = new RouteData();
        routeData.Values.Add("id", validId);

        var httpContext = new DefaultHttpContext();
        httpContext.Features.Set<IRoutingFeature>(new RoutingFeature { RouteData = routeData });
        httpContext.Request.Method = HttpMethod.Put.Method;

        _mockHttpContextAccessor.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        var validator = new ProductRouteValidator(_mockHttpContextAccessor.Object);

        // Act & Assert
        var result = validator.TestValidate(new ProductRequest());
        result.ShouldNotHaveValidationErrorFor("id");
    }
}