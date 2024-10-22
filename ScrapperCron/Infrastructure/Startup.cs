using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ScrapperCron.Services;

namespace ScrapperCron.Infrastructure
{
    public static class Startup
    {
        // Configure Serilog
        public static void ConfigureLogging()
        {
            // Setup Serilog using configuration from appsettings.cron.json
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(LoadConfiguration()) // Load configuration from the file
                .CreateLogger();

            Log.Information("Logging configured successfully.");
        }

        // Method to load configuration from appsettings.cron.json
        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.cron.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        // Create and configure the host builder
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Use Serilog as the logging provider
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Setup configuration loading
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.cron.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Register configuration and services
                    var configuration = LoadConfiguration();
                    services.AddSingleton(configuration);
                    services.AddScoped<ICronService, CronService>();
                    services.AddHostedService<CronHostedService>();
                })
                .UseWindowsService(); // This indicates that the application will run as a Windows Service
    }
}
