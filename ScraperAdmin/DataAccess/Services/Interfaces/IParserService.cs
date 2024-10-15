using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Models.Documents;
using ScraperAdmin.DataAccess.Models.DTOs;

namespace ScraperAdmin.DataAccess.Services
{
      public interface IParserService
    {
        Task<ParseResult> ParseHtmlToEventsAsync(RawHtmlDocument document);
    }
}