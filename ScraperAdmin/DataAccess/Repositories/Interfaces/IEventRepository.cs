using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.DataAccess.Repositories
{
     public interface IEventRepository
    {
        Task<List<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(string id);
        Task CreateEventAsync(Event @event);
        Task CreateEventsAsync(IEnumerable<Event> events);
        Task UpdateEventAsync(string id, Event @event);
        Task DeleteEventAsync(string id);
        Task<List<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> ScraperExistsAsync(Guid scraperId);
    }
}