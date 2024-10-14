using ScraperAdmin.DataAccess.Models.Documents;

namespace ScraperAdmin.DataAccess.Services
{
    
    public interface IAIEventProcessingService
    {
        Task ProcessAndStoreEventsAsync();
    }
}