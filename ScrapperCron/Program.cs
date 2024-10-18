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
            // Crea el host que manejará tu aplicación como un servicio de Windows
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Cargar la configuración de appsettings.cron.json
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.cron.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Configurar las dependencias
                    var configuration = LoadConfiguration(); // Usa tu método existente para cargar la configuración
                    services.AddSingleton<IConfiguration>(configuration);
                    services.AddScoped<ICronService, CronService>(); // Mantén la inyección de dependencia

                    // Registrar el servicio hospedado que correrá el CronService
                    services.AddHostedService<CronHostedService>();
                })
                .UseWindowsService() // Esto indica que la aplicación será un servicio de Windows
                .Build();

            // Ejecutar el servicio
            await host.RunAsync();
        }

        // Método para cargar la configuración desde appsettings.cron.json (esto es igual al original)
        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.cron.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
