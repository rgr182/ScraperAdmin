namespace ScraperAdmin.DataAccess.Services
{

    public interface IAIEventProcessingService
    {
        Task ProcessAndStoreEventsAsync();
    }
}