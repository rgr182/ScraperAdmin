using ScraperAdmin.DataAccess.Models.Documents;
using ScraperAdmin.DataAccess.Repositories;
namespace ScraperAdmin.DataAccess.Services{
public class RawHtmlService : IRawHtmlService
{
    private readonly IRawHtmlRepository _rawHtmlRepository;
    private readonly ILogger<RawHtmlService> _logger;

    public RawHtmlService(IRawHtmlRepository rawHtmlRepository, ILogger<RawHtmlService> logger)
    {
        _rawHtmlRepository = rawHtmlRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<RawHtmlDocument>> GetAllRawHtmlAsync()
    {
        try
        {
            var documents = await _rawHtmlRepository.GetAllRawHtmlAsync();
            _logger.LogInformation($"Retrieved {documents.Count} raw HTML documents");
            return documents;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching raw HTML documents");
            throw;
        }
    }
}

}
