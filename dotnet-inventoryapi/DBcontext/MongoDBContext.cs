using dotnet_inventoryapi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace dotnet_inventoryapi.DBcontext
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IOptions<MongoDBSettings> mongoDBSettings)
        {

            if (string.IsNullOrWhiteSpace(mongoDBSettings.Value.ConnectionString))
            {
                throw new ArgumentException("MongoDB connection string is missing or empty.");
            }

            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            if (_database == null)
            {
                throw new InvalidOperationException("MongoDB database is null after initialization.");
            }
        }


        public IMongoCollection<Product> Products => _database.GetCollection<Product>("products");
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");

        public IMongoCollection<Categories> Categories => _database.GetCollection<Categories>("categories");
    }
}
