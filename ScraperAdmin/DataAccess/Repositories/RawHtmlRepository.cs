using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.DataAccess.Repositories
{
   

    public class RawHtmlRepository : IRawHtmlRepository
    {
        private readonly IMongoCollection<RawHtmlDocument> _rawHtmlCollection;
        private readonly ILogger<RawHtmlRepository> _logger;

        public RawHtmlRepository(
            MongoDbContext context,
            IOptions<RawHtmlRepositoryOptions> options,
            ILogger<RawHtmlRepository> logger)
        {
            if (string.IsNullOrWhiteSpace(options.Value.CollectionName))
            {
                throw new ArgumentException("Collection name must be specified in options.", nameof(options));
            }

            _rawHtmlCollection = context.GetCollection<RawHtmlDocument>(options.Value.CollectionName);
            _logger = logger;

            _logger.LogInformation("RawHtmlRepository initialized with collection: {CollectionName}", options.Value.CollectionName);
        }

        public async Task<List<RawHtmlDocument>> GetAllUnparsedRawHtmlAsync()
        {
            try
            {
                var documents = await _rawHtmlCollection.Find(d => !d.IsParsed).ToListAsync();
                _logger.LogInformation("Retrieved {Count} unparsed raw HTML documents", documents.Count);
                return documents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching unparsed raw HTML documents");
                throw;
            }
        }

        public async Task UpdateParsedStatusAsync(string id, bool isParsed)
        {
            try
            {
                var filter = Builders<RawHtmlDocument>.Filter.Eq(d => d.Id, id);
                var update = Builders<RawHtmlDocument>.Update.Set(d => d.IsParsed, isParsed);
                var result = await _rawHtmlCollection.UpdateOneAsync(filter, update);
                _logger.LogInformation("Updated parsed status for document {Id}. Modified: {ModifiedCount}", id, result.ModifiedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating parsed status for document {Id}", id);
                throw;
            }
        }
    }
     public class RawHtmlRepositoryOptions
    {
        public const string RawHtml = "RawHtml";

        public string CollectionName { get; set; } = string.Empty;
    }
}