using Moq;
using DenerViana.Ctt.Product.Api.Domain.Services;
using DenerViana.Ctt.Product.Api.Domain.Interfaces;
using DenerViana.Ctt.Product.Api.Domain.Entities;
using FizzWare.NBuilder;

public class ProductServicesTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly ProductServices _productServices;

    public ProductServicesTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _productServices = new ProductServices(_mockProductRepository.Object);
    }

    [Fact(DisplayName = "Return list of products")]
    [Trait("Domain", "ProductServices")]
    public async Task GetAllAsync_ReturnsListOfProducts()
    {
        // Arrange
        var mockProducts = Builder<Product>.CreateListOfSize(10).All().Build();

        _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockProducts);

        // Act
        var result = await _productServices.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Count());
        _mockProductRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact(DisplayName = "Return product when id exists")]
    [Trait("Domain", "ProductServices")]
    public async Task GetByIdAsync_ReturnsProduct_WhenIdExists()
    {
        // Arrange
        var mockProduct = Builder<Product>.CreateNew().Build();

        _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockProduct);

        // Act
        var result = await _productServices.GetByIdAsync(It.IsAny<Guid>());

        // Assert
        Assert.NotNull(result);
        _mockProductRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact(DisplayName = "Returns true when product exists by id")]
    [Trait("Domain", "ProductServices")]
    public async Task ExistsAsync_ReturnsTrue_WhenProductExistsById()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await _productServices.ExistsAsync(It.IsAny<Guid>());

        // Assert
        Assert.True(result);
        _mockProductRepository.Verify(repo => repo.ExistsAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact(DisplayName = "Returns false when product does not exist by id")]
    [Trait("Domain", "ProductServices")]
    public async Task ExistsAsync_ReturnsFalse_WhenProductDoesNotExistById()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await _productServices.ExistsAsync(It.IsAny<Guid>());

        // Assert
        Assert.False(result);
        _mockProductRepository.Verify(repo => repo.ExistsAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact(DisplayName = "Returns true when product exists description")]
    [Trait("Domain", "ProductServices")]
    public async Task ExistsAsync_ReturnsTrue_WhenProductExistsByDescription()
    {
        // Arrange
        var description = "Product 1";
        _mockProductRepository.Setup(repo => repo.ExistsAsync(description)).ReturnsAsync(true);

        // Act
        var result = await _productServices.ExistsAsync(description);

        // Assert
        Assert.True(result);
        _mockProductRepository.Verify(repo => repo.ExistsAsync(description), Times.Once);
    }

    [Fact(DisplayName = "Returns true when product is added successfully")]
    [Trait("Domain", "ProductServices")]
    public async Task AddAsync_ReturnsTrue_WhenProductIsAddedSuccessfully()
    {
        // Arrange
        var mockProduct = Builder<Product>.CreateNew().Build();


        _mockProductRepository.Setup(repo => repo.AddAsync(mockProduct)).ReturnsAsync(true);

        // Act
        var result = await _productServices.AddAsync(mockProduct);

        // Assert
        Assert.True(result);
        _mockProductRepository.Verify(repo => repo.AddAsync(mockProduct), Times.Once);
    }

    [Fact(DisplayName = "Returns false when product addition fails")]
    [Trait("Domain", "ProductServices")]
    public async Task AddAsync_ReturnsFalse_WhenProductAdditionFails()
    {
        // Arrange
        var mockProduct = Builder<Product>.CreateNew().Build();

        _mockProductRepository.Setup(repo => repo.AddAsync(mockProduct)).ReturnsAsync(false);

        // Act
        var result = await _productServices.AddAsync(mockProduct);

        // Assert
        Assert.False(result);
        _mockProductRepository.Verify(repo => repo.AddAsync(mockProduct), Times.Once);
    }
}