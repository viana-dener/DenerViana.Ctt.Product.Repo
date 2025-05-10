using Moq;
using Microsoft.AspNetCore.Http;
using DenerViana.Ctt.Product.Api.Application.Interfaces;
using DenerViana.Ctt.Product.Api.Application.Models.Request;
using DenerViana.Ctt.Product.Api.Application.Models.Response;
using DenerViana.Ctt.Product.Api.Base.Interfaces;
using FizzWare.NBuilder;
using DenerViana.Ctt.Product.Api.Domain.Dtos;

namespace DenerViana.Ctt.Product.Test.Presentation;

public class ProductEndpointsTests
{
    private readonly Mock<IProductAppServices> _mockProductAppServices;
    private readonly Mock<IMainEndpoints> _mockMainEndpoints;
    private readonly Mock<INotify> _mockNotify;

    public ProductEndpointsTests()
    {
        _mockProductAppServices = new Mock<IProductAppServices>();
        _mockMainEndpoints = new Mock<IMainEndpoints>();
        _mockNotify = new Mock<INotify>();
    }

    [Fact(DisplayName = "Get all Products")]
    [Trait("Presentation", null)]
    public async Task GetAllProducts_ReturnsProducts()
    {
        // Arrange
        var mockProducts = Builder<ProductResponse>.CreateListOfSize(5).Build();
        _mockProductAppServices.Setup(service => service.GetAllAsync()).ReturnsAsync(mockProducts);

        // Act
        var result = await _mockProductAppServices.Object.GetAllAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "Get product by id")]
    [Trait("Presentation", null)]
    public async Task GetProductById_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var mockResponse = Builder<ProductDetailsResponse>.CreateNew().With(p => p.Id = productId.ToString()).Build();

        _mockProductAppServices.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync(mockResponse);

        // Act
        var result = await _mockProductAppServices.Object.GetByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "Add a product")]
    [Trait("Presentation", null)]
    public async Task AddProduct_ReturnsSuccess_WhenHeadersAreValid()
    {
        // Arrange
        var productRequest = Builder<ProductRequest>.CreateNew().Build();
        var mockResponse = Builder<GenericResponse>.CreateNew().With(p => p.Result = true).Build().Result;
        var mockHeaders = new HeaderDictionary
        {
            { "x-userid", "test-user" },
            { "x-correlationid", Guid.NewGuid().ToString() }
        };

        var mockUserInfo = Builder<UserInfoDto>.CreateNew().Build();

        _mockMainEndpoints.Setup(endpoint => endpoint.HttpContext.Request.Headers).Returns(mockHeaders);
        _mockProductAppServices.Setup(service => service.AddAsync(productRequest, mockUserInfo)).ReturnsAsync(mockResponse);

        // Act
        var result = await _mockProductAppServices.Object.AddAsync(productRequest, mockUserInfo);

        // Assert
        Assert.True(result);
    }
}