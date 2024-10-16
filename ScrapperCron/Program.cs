using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrapperCron.Services;
using Cron_BolsaDeTrabajo.Infrastructure;

namespace Cron_BolsaDeTrabajo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load configuration from appsettings.json
            var configuration = LoadConfiguration();

            // Setup Dependency Injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IMongoDbConnection>(sp =>
                {
                    var mongoConnectionString = configuration["MongoDB:ConnectionString"];
                    var mongoDatabaseName = configuration["MongoDB:DatabaseName"];
                    return new MongoDbConnection(mongoConnectionString, mongoDatabaseName);
                })                
                .AddSingleton<ICronService, CronService>()
                .BuildServiceProvider();

            var cronService = serviceProvider.GetService<ICronService>();

#if TESTING
            // Execute testing method if in testing profile
            cronService.TestCron().Wait();
#else
            // Start the Cron Service
            cronService.StartAsync().Wait();
#endif

            // Keep the application running
            Console.WriteLine("Press [Enter] to exit the program.");
            Console.ReadLine();
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.cron.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
