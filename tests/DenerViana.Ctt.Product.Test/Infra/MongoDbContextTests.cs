using Moq;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DenerViana.Ctt.Product.Api.Infra.Context;
using DenerViana.Ctt.Product.Api.Base;

public class MongoDbContextTests
{
    private readonly Mock<IOptions<MongoDbSettings>> _mockMongoDbSettings;
    private readonly MongoDbSettings _settings;

    public MongoDbContextTests()
    {
        _settings = new MongoDbSettings
        {
            MongoDbConnection = "mongodb://localhost:27017",
            DatabaseName = "TestDatabase"
        };

        _mockMongoDbSettings = new Mock<IOptions<MongoDbSettings>>();
        _mockMongoDbSettings.Setup(s => s.Value).Returns(_settings);
    }

    [Fact(DisplayName = "Initializes Database Connection")]
    [Trait("Infra", "MongoDbContext")]
    public void MongoDbContext_InitializesDatabaseConnection()
    {
        // Act
        var context = new MongoDbContext(_mockMongoDbSettings.Object);

        // Assert
        Assert.NotNull(context);
    }

    [Fact(DisplayName = "Returns Correct Collection")]
    [Trait("Infra", "MongoDbContext")]
    public void ProductsCollection_ReturnsCorrectCollection()
    {
        // Arrange
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockClient = new Mock<IMongoClient>();
        var mockCollection = new Mock<IMongoCollection<DenerViana.Ctt.Product.Api.Domain.Entities.Product>>();

        mockDatabase.Setup(db => db.GetCollection<DenerViana.Ctt.Product.Api.Domain.Entities.Product>("products", null))
                    .Returns(mockCollection.Object);

        mockClient.Setup(client => client.GetDatabase(_settings.DatabaseName, null))
                  .Returns(mockDatabase.Object);

        var mongoSettings = MongoClientSettings.FromConnectionString(_settings.MongoDbConnection);
        var client = new MongoClient(mongoSettings);
        var context = new MongoDbContext(_mockMongoDbSettings.Object);

        // Act
        var collection = context.Products;

        // Assert
        Assert.NotNull(collection);
    }

    [Fact(DisplayName = "Logs Connection Message")]
    [Trait("Infra", "MongoDbContext")]
    public void MongoDbContext_LogsConnectionMessage()
    {
        // Arrange
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        var context = new MongoDbContext(_mockMongoDbSettings.Object);

        // Assert
        Assert.Contains($"Conectado ao MongoDB: {_settings.MongoDbConnection}", consoleOutput.ToString());
    }
}