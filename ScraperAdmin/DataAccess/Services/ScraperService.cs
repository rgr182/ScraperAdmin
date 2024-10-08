using ScraperAdmin.DataAccess.Repositories;

namespace ScraperAdmin.DataAccess.Services
{
    public interface IScraperService
    {
        Task<IEnumerable<Scraper>> GetAllScrapersAsync(HttpRequest request);
        Task<Scraper> GetScraperByIdAsync(Guid scraperId, HttpRequest request);
    }

    public class ScraperService : IScraperService
    {
        private readonly IScraperRepository _scraperRepository;

        public ScraperService(IScraperRepository scraperRepository)
        {
            _scraperRepository = scraperRepository;
        }

        public async Task<IEnumerable<Scraper>> GetAllScrapersAsync(HttpRequest request)
        {
            var scrapers = await _scraperRepository.GetAllAsync();

            foreach (var scraper in scrapers)
            {
                scraper.ImagePath = $"{request.Scheme}://{request.Host}{scraper.ImagePath}";
            }

            return scrapers;
        }

        public async Task<Scraper> GetScraperByIdAsync(Guid scraperId, HttpRequest request)
        {
            var scraper = await _scraperRepository.GetByIdAsync(scraperId);

            if (scraper != null)
            {
                scraper.ImagePath = $"{request.Scheme}://{request.Host}{scraper.ImagePath}";
            }

            return scraper;
        }
    }
}
