using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScrapperCron.Services;
using System;
using System.Threading.Tasks;

namespace Cron_BolsaDeTrabajo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.cron.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = LoadConfiguration();
                    services.AddSingleton<IConfiguration>(configuration);
                    services.AddScoped<ICronService, CronService>();
                    services.AddHostedService<CronHostedService>();
                })
                .UseWindowsService()
                .Build();
            await host.RunAsync();
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
