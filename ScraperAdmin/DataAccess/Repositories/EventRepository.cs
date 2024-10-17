using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Models.Documents;
using MongoDB.Driver;

namespace ScraperAdmin.DataAccess.Repositories
{


    public class EventRepository : IEventRepository
    {
        private readonly MongoDbContext _context;

        public EventRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllEventsAsync() =>
            await _context.Events.Find(_ => true).ToListAsync();

        public async Task<Event> GetEventByIdAsync(string id) =>
            await _context.Events.Find<Event>(e => e.Id == id).FirstOrDefaultAsync();

        public async Task CreateEventAsync(Event @event) =>
            await _context.Events.InsertOneAsync(@event);

        public async Task CreateEventsAsync(IEnumerable<Event> events) =>
            await _context.Events.InsertManyAsync(events);

        public async Task UpdateEventAsync(string id, Event @event) =>
            await _context.Events.ReplaceOneAsync(e => e.Id == id, @event);

        public async Task DeleteEventAsync(string id) =>
            await _context.Events.DeleteOneAsync(e => e.Id == id);

        public async Task<List<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate) =>
            await _context.Events.Find(e => e.Date >= startDate && e.Date <= endDate).ToListAsync();

        public async Task<bool> ScraperExistsAsync(Guid scraperId)
        {
            var scraper = await _context.Scrapers.Find(s => s.ScraperId == scraperId).FirstOrDefaultAsync();
            return scraper != null;
        }

        public async Task CreateEventsAsync(IEnumerable<Event> events, Guid scraperId)
        {
            if (!await ScraperExistsAsync(scraperId))
            {
                throw new InvalidOperationException($"Scraper with ID {scraperId} does not exist.");
            }
            await _context.Events.InsertManyAsync(events);
        }
    }
}