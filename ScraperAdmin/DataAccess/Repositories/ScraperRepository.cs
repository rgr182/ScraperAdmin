using Microsoft.EntityFrameworkCore;
using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Models;

namespace ScraperAdmin.DataAccess.Repositories
{
    public interface IScraperRepository
    {
        Task<IEnumerable<Scraper>> GetAllAsync();
        Task<Scraper> GetByIdAsync(Guid scraperId);
        Task AddAsync(Scraper scraper);
        Task UpdateAsync(Scraper scraper);
        Task DeleteAsync(Guid scraperId);
    }

    public class ScraperRepository : IScraperRepository
    {
        private readonly ApplicationDbContext _context;

        public ScraperRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Scraper>> GetAllAsync()
        {
            return await _context.Scraper.ToListAsync();
        }

        public async Task<Scraper> GetByIdAsync(Guid scraperId)
        {
            return await _context.Scraper.FindAsync(scraperId);
        }

        public async Task AddAsync(Scraper scraper)
        {
            await _context.Scraper.AddAsync(scraper);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Scraper scraper)
        {
            _context.Scraper.Update(scraper);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid scraperId)
        {
            var scraper = await _context.Scraper.FindAsync(scraperId);
            if (scraper != null)
            {
                _context.Scraper.Remove(scraper);
                await _context.SaveChangesAsync();
            }
        }
    }
}
