using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.DataAccess.Services{   
public interface IRawHtmlService
{
    Task<IEnumerable<RawHtmlDocument>> GetAllRawHtmlAsync();
}
}