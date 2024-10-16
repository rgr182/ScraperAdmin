using MongoDB.Driver;
using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.DataAccess.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
        public IMongoCollection<Scraper> Scrapers => _database.GetCollection<Scraper>("Scrapers");
    }
}