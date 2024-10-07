using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperAdmin.DataAccess.Services
{
    public interface IScraperService
    {
        Task<IEnumerable<Scraper>> GetAllScrapersAsync();
        Task<Scraper> GetScraperByIdAsync(Guid scraperId);
        Task CreateScraperAsync(Scraper scraper);
        Task UpdateScraperAsync(Scraper scraper);
        Task DeleteScraperAsync(Guid scraperId);
    }

    public class ScraperService : IScraperService
    {
        private readonly IScraperRepository _scraperRepository;

        public ScraperService(IScraperRepository scraperRepository)
        {
            _scraperRepository = scraperRepository;
        }

        public async Task<IEnumerable<Scraper>> GetAllScrapersAsync()
        {
            return await _scraperRepository.GetAllAsync();
        }

        public async Task<Scraper> GetScraperByIdAsync(Guid scraperId)
        {
            return await _scraperRepository.GetByIdAsync(scraperId);
        }

        public async Task CreateScraperAsync(Scraper scraper)
        {
            // Generate a new ID for the scraper
            scraper.ScraperId = Guid.NewGuid();
            await _scraperRepository.AddAsync(scraper);
        }

        public async Task UpdateScraperAsync(Scraper scraper)
        {
            await _scraperRepository.UpdateAsync(scraper);
        }

        public async Task DeleteScraperAsync(Guid scraperId)
        {
            await _scraperRepository.DeleteAsync(scraperId);
        }
    }
}
