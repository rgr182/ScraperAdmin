using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Repositories;

namespace ScraperAdmin.DataAccess.Services
{
    public interface IScraperService
    {
        Task<IEnumerable<Scraper>> GetAllScrapersAsync(HttpRequest request);
        Task<Scraper?> GetScraperByIdAsync(Guid scraperId, HttpRequest request);
        Task<Scraper> CreateScraperAsync(Scraper scraper, HttpRequest request);
        Task<bool> UpdateLastExecutionDateAsync(Guid scraperId, DateTime lastExecutionDate);
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

        public async Task<Scraper?> GetScraperByIdAsync(Guid scraperId, HttpRequest request)
        {
            var scraper = await _scraperRepository.GetByIdAsync(scraperId);

            if (scraper != null)
            {
                scraper.ImagePath = $"{request.Scheme}://{request.Host}{scraper.ImagePath}";
            }           

            return scraper;
        }

        public async Task<Scraper> CreateScraperAsync(Scraper scraper, HttpRequest request)
        {
            scraper.ScraperId = Guid.NewGuid();
            scraper.LastExecutionDate = null;  // Set to null as it's a new scraper

            var createdScraper = await _scraperRepository.AddAsync(scraper);

            // Update the ImagePath with the full URL
            createdScraper.ImagePath = $"{request.Scheme}://{request.Host}{createdScraper.ImagePath}";

            return createdScraper;
        }

        public async Task<bool> UpdateLastExecutionDateAsync(Guid scraperId, DateTime lastExecutionDate)
        {
            return await _scraperRepository.UpdateLastExecutionDateAsync(scraperId, lastExecutionDate);
        }
    }
}