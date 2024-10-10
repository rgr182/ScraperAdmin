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

        public async Task<List<RawHtmlDocument>> GetAllRawHtmlAsync()
        {
            try
            {
                var documents = await _rawHtmlCollection.Find(_ => true).ToListAsync();
                _logger.LogInformation("Retrieved {Count} raw HTML documents", documents.Count);
                return documents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching raw HTML documents");
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