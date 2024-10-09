using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.DataAccess.Repositories
{
    public interface IRawHtmlRepository
{
    Task<List<RawHtmlDocument>> GetAllRawHtmlAsync();
}
}