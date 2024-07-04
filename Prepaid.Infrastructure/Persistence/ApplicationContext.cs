using MongoDB.Driver;
using Prepaid.Domain.Models;
using Prepaid.Infrastructure.Configurations;

namespace Prepaid.Infrastructure.Persistence;

public class ApplicationContext
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly MongoDbConfiguration _mongoDbConfiguration;

    public ApplicationContext(MongoDbConfiguration mongoDbConfiguration)
    {
        _mongoClient = new MongoClient(_mongoDbConfiguration?.ConnectionString);
        _mongoDbConfiguration = mongoDbConfiguration;
        _mongoDatabase = _mongoClient.GetDatabase(mongoDbConfiguration.Database);
    }

    public IMongoCollection<Booking> Bookings => _mongoDatabase.GetCollection<Booking>("bookings");
}