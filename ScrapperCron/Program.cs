using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrapperCron.Services;

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
                .AddScoped<ICronService, CronService>()
                .BuildServiceProvider();

            var cronService = serviceProvider.GetService<ICronService>();

#if TESTING
            // Execute testing method if in testing profile
            cronService.ExecuteOnce().Wait();
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
