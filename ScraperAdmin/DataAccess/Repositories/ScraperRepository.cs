using MongoDB.Driver;
using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Models;

namespace ScraperAdmin.DataAccess.Repositories
{
    public interface IScraperRepository
    {
        Task<IEnumerable<Scraper>> GetAllAsync();
        Task<Scraper> GetByIdAsync(Guid scraperId);
        Task<Scraper> AddAsync(Scraper scraper);
        Task UpdateAsync(Scraper scraper);
        Task<bool> UpdateLastExecutionDateAsync(Guid scraperId, DateTime lastExecutionDate);
        Task DeleteAsync(Guid scraperId);
    }

    public class ScraperRepository : IScraperRepository
    {
        private readonly IMongoCollection<Scraper> _scrapers;

        public ScraperRepository(MongoDbContext context)
        {
            _scrapers = context.Scrapers;
        }

        public async Task<IEnumerable<Scraper>> GetAllAsync()
        {
            return await _scrapers.Find(_ => true).ToListAsync();
        }

        public async Task<Scraper> GetByIdAsync(Guid scraperId)
        {
            return await _scrapers.Find(s => s.ScraperId == scraperId).FirstOrDefaultAsync();
        }

        public async Task<Scraper> AddAsync(Scraper scraper)
        {
            await _scrapers.InsertOneAsync(scraper);
            return scraper;
        }

        public async Task UpdateAsync(Scraper scraper)
        {
            await _scrapers.ReplaceOneAsync(s => s.ScraperId == scraper.ScraperId, scraper);
        }

        public async Task<bool> UpdateLastExecutionDateAsync(Guid scraperId, DateTime lastExecutionDate)
        {
            var updateDefinition = Builders<Scraper>.Update.Set(s => s.LastExecutionDate, lastExecutionDate);
            var result = await _scrapers.UpdateOneAsync(s => s.ScraperId == scraperId, updateDefinition);
            return result.ModifiedCount > 0;
        }

        public async Task DeleteAsync(Guid scraperId)
        {
            await _scrapers.DeleteOneAsync(s => s.ScraperId == scraperId);
        }
    }
}