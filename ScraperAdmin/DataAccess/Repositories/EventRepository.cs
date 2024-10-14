using MongoDB.Driver;
using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.DataAccess.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IMongoCollection<Event> _events;

        public EventRepository(MongoDbContext context)
        {
            _events = context.Events;
        }

        public async Task<List<Event>> GetAllEventsAsync() =>
            await _events.Find(_ => true).ToListAsync();

        public async Task<Event> GetEventByIdAsync(string id) =>
            await _events.Find<Event>(e => e.Id == id).FirstOrDefaultAsync();

        public async Task CreateEventAsync(Event @event) =>
            await _events.InsertOneAsync(@event);

         public async Task CreateEventsAsync(IEnumerable<Event> events)
        {
            await _events.InsertManyAsync(events);
        }
        public async Task UpdateEventAsync(string id, Event @event) =>
            await _events.ReplaceOneAsync(e => e.Id == id, @event);

        public async Task DeleteEventAsync(string id) =>
            await _events.DeleteOneAsync(e => e.Id == id);

        public async Task<List<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate) =>
            await _events.Find(e => e.Date >= startDate && e.Date <= endDate).ToListAsync();
    }
}