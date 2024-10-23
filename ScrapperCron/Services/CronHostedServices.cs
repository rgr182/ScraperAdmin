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
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _cronService.StartAsync();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
