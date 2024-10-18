using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace ScrapperCron.Services
{
    public class CronHostedService : BackgroundService
    {
        private readonly ICronService _cronService;

        public CronHostedService(ICronService cronService)
        {
            _cronService = cronService;
        }

        // Este método se ejecuta cuando el servicio se inicia
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _cronService.StartAsync(); // Inicia el cron job
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken); // Mantener la ejecución en segundo plano
            }
        }

        // Detener el servicio cuando se recibe la señal de parada
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
