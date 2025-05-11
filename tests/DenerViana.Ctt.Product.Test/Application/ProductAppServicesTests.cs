using Moq;
using FizzWare.NBuilder;
using DenerViana.Ctt.Product.Api.Application.Services;
using DenerViana.Ctt.Product.Api.Application.Models.Request;
using DenerViana.Ctt.Product.Api.Application.Models.Response;
using DenerViana.Ctt.Product.Api.Domain.Interfaces;
using DenerViana.Ctt.Product.Api.Domain.Entities;
using DenerViana.Ctt.Product.Api.Domain.Dtos;
using AutoMapper;
using DenerViana.Ctt.Product.Api.Base.Interfaces;
using DenerViana.Ctt.Product.Api.Application.Interfaces;

public class ProductAppServicesTests
{
    private readonly Mock<IProductServices> _mockProductServices;
    private readonly Mock<INotify> _mockNotify;
    private readonly Mock<ILogInformation> _mockLogInformation;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ProductAppServices _productAppServices;

    public ProductAppServicesTests()
    {
        _mockProductServices = new Mock<IProductServices>();
        _mockNotify = new Mock<INotify>();
        _mockLogInformation = new Mock<ILogInformation>();
        _mockMapper = new Mock<IMapper>();

        _productAppServices = new ProductAppServices(
            _mockLogInformation.Object,
            _mockNotify.Object,
            _mockMapper.Object,
            _mockProductServices.Object
        );
    }

    [Fact(DisplayName = "Return mapped products")]
    [Trait("Application", "ProductAppServices")]
    public async Task GetAllAsync_ReturnsMappedProducts()
    {
        // Arrange
        var mockProducts = Builder<Product>.CreateListOfSize(5).Build();
        var mockResponse = Builder<ProductResponse>.CreateListOfSize(5).Build();

        _mockProductServices.Setup(s => s.GetAllAsync()).ReturnsAsync(mockProducts);
        _mockMapper.Setup(m => m.Map<IEnumerable<ProductResponse>>(mockProducts)).Returns(mockResponse);

        // Act
        var result = await _productAppServices.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
        _mockMapper.Verify(m => m.Map<IEnumerable<ProductResponse>>(mockProducts), Times.Once);
    }

    [Fact(DisplayName = "Returns mapped product when product exists")]
    [Trait("Application", "ProductAppServices")]
    public async Task GetByIdAsync_ReturnsMappedProduct_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var mockProduct = Builder<Product>.CreateNew().Build();
        var mockResponse = Builder<ProductDetailsResponse>.CreateNew().Build();

        _mockProductServices.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync(mockProduct);
        _mockMapper.Setup(m => m.Map<ProductDetailsResponse>(mockProduct)).Returns(mockResponse);

        // Act
        var result = await _productAppServices.GetByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        _mockMapper.Verify(m => m.Map<ProductDetailsResponse>(mockProduct), Times.Once);
    }

    [Fact(DisplayName = "Returns false when product already exists")]
    [Trait("Application", "ProductAppServices")]
    public async Task AddAsync_ReturnsFalse_WhenProductAlreadyExists()
    {
        // Arrange
        var mockRequest = Builder<ProductRequest>.CreateNew().Build();
        var mockUserInfo = Builder<UserInfoDto>.CreateNew().Build();

        _mockProductServices.Setup(s => s.ExistsAsync(mockRequest.Description)).ReturnsAsync(true);

        // Act
        var result = await _productAppServices.AddAsync(mockRequest, mockUserInfo);

        // Assert
        Assert.False(result);
        _mockNotify.Verify(n => n.AddError("Product already exists"), Times.Once);
    }

    [Fact(DisplayName = "Adds product when product does not exist")]
    [Trait("Application", "ProductAppServices")]
    public async Task AddAsync_AddsProduct_WhenProductDoesNotExist()
    {
        // Arrange
        var mockRequest = Builder<ProductRequest>.CreateNew().Build();
        var mockUserInfo = Builder<UserInfoDto>.CreateNew().Build();
        var mockProduct = Builder<Product>.CreateNew().Build();

        _mockProductServices.Setup(s => s.ExistsAsync(mockRequest.Description)).ReturnsAsync(false);
        _mockProductServices.Setup(s => s.AddAsync(It.IsAny<Product>())).ReturnsAsync(true);

        // Act
        var result = await _productAppServices.AddAsync(mockRequest, mockUserInfo);

        // Assert
        Assert.True(result);
        _mockProductServices.Verify(s => s.AddAsync(It.IsAny<Product>()), Times.Once);
    }
}