using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.DataAccess.Repositories
{
   public interface IRawHtmlRepository
    {
        Task<List<RawHtmlDocument>> GetAllUnparsedRawHtmlAsync();
        Task UpdateParsedStatusAsync(string id, bool isParsed);
    }
}