using DenerViana.Ctt.Product.Api.Base;
using DenerViana.Ctt.Product.Api.Domain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DenerViana.Ctt.Product.Api.Infra.Context;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var mongoSettings = MongoClientSettings.FromConnectionString(settings.Value.MongoDbConnection);

        // Define a representação padrão de Guid como Standard
        mongoSettings.GuidRepresentation = GuidRepresentation.Standard;

        var client = new MongoClient(mongoSettings);
        _database = client.GetDatabase(settings.Value.DatabaseName);

        // Adicione um log para garantir que a conexão foi estabelecida
        Console.WriteLine($"Conectado ao MongoDB: {settings.Value.MongoDbConnection}");
    }

    public IMongoCollection<Domain.Entities.Product> Products => _database.GetCollection<Domain.Entities.Product>("products");
}
