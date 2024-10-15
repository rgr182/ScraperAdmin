using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Cron_BolsaDeTrabajo.Infrastructure;
using NCrontab;

namespace ScrapperCron.Services
{
    public interface ICronService
    {
        Task StartAsync();
        Task ExecuteOnce();
    }

    public class CronService : ICronService
    {
        private readonly IMongoDbConnection _mongoDbConnection;        
        private Timer _timer;
        private readonly string _cronExpression;
        private readonly IConfiguration _configuration;

        public CronService(IMongoDbConnection mongoDbConnection, IConfiguration configuration)
        {
            _mongoDbConnection = mongoDbConnection;
            _configuration = configuration;

            // Setup MongoDB collection access
            var mongoCollectionName = _configuration["MongoDB:CollectionName"];         

            // Load cron expression from configuration
            _cronExpression = _configuration["CronJob:CronExpression"];

            if (string.IsNullOrEmpty(_cronExpression))
            {
                Console.WriteLine("No valid cron expression found in the configuration.");
                return;
            }

#if !TESTING
            // In normal mode, setup the cron job
            InitializeCronJob();
#endif
        }

        private void InitializeCronJob()
        {
            // Initialize timer based on the cron expression
            TimeSpan timeUntilNextRun = CalculateTimeUntilNextRun(_cronExpression);
            _timer = new Timer(async _ => await ExecuteTaskAsync(), null, timeUntilNextRun, Timeout.InfiniteTimeSpan);
            Console.WriteLine("Cron job initialized.");
        }

        public async Task StartAsync()
        {
            Console.WriteLine("Cron job Program started.");
            // Further implementation to start the cron job can be added here
        }

        public async Task ExecuteOnce()
        {
            await ExecuteTaskAsync();            
        }

        private async Task ExecuteTaskAsync()
        {
            Console.WriteLine("Ejecutado!");
            //TODO: Execute Cron.

            // Reset the timer for the next run according to the cron expression
            TimeSpan timeUntilNextRun = CalculateTimeUntilNextRun(_cronExpression);
            _timer.Change(timeUntilNextRun, Timeout.InfiniteTimeSpan);
        }

        private TimeSpan CalculateTimeUntilNextRun(string cronExpression)
        {
            var cronSchedule = CrontabSchedule.Parse(cronExpression);
            DateTime now = DateTime.Now;
            DateTime nextRun = cronSchedule.GetNextOccurrence(now);

            return nextRun - now;
        }
    }
}
