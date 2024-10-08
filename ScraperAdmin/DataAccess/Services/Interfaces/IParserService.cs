using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.Services
{
    public interface IParserService
    {
        Task<List<Event>> ParseHtmlToEventsAsync(string htmlContent);
    }
}