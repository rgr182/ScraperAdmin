using MongoDB.Driver;
using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.DataAccess.Repositories{
    public class RawHtmlRepository : IRawHtmlRepository
{
    private readonly IMongoCollection<RawHtmlDocument> _rawHtmlCollection;

    public RawHtmlRepository(MongoDbContext context)
    {
        _rawHtmlCollection = context.GetCollection<RawHtmlDocument>("bookspider_2024_10_09");
  
    }

    public async Task<List<RawHtmlDocument>> GetAllRawHtmlAsync() =>
        await _rawHtmlCollection.Find(_ => true).ToListAsync();
}

}
