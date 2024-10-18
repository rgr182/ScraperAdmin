using System.Diagnostics;
using Microsoft.Extensions.Configuration;
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
        private Timer _timer;
        private readonly string _cronExpression;
        private readonly IConfiguration _configuration;
        private readonly string _virtualEnvPath;
        private readonly string _scrapyProjectPath;
        private readonly string _executeSpiderPath;

        public CronService(IConfiguration configuration)
        {            
            _configuration = configuration;

            // Setup MongoDB collection access
            var mongoCollectionName = _configuration["MongoDB:CollectionName"];         

            // Load cron expression from configuration
            _cronExpression = _configuration["CronJob:CronExpression"];
            _virtualEnvPath = _configuration["PythonSettings:VirtualEnvPath"];
            _scrapyProjectPath = _configuration["PythonSettings:ScrapyProjectPath"];
            _executeSpiderPath = _configuration["PythonSettings:ExecuteSpiderPath"];
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
            TimeSpan timeUntilNextRun = CalculateTimeUntilNextRun(_cronExpression);
            _timer = new Timer(async _ => await ExecuteTaskAsync(), null, timeUntilNextRun, Timeout.InfiniteTimeSpan);
            Console.WriteLine("Cron job initialized.");
        }
        public async Task StartAsync()
        {
            Console.WriteLine("Cron job Program started.");
        }
        public async Task ExecuteOnce()
        {
            await ExecuteTaskAsync(false);            
        }
        private async Task ExecuteTaskAsync(bool isCron = true)
        {
            Console.WriteLine("Activando entorno virtual y cambiando al directorio del proyecto...");

            var activateProcessStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {_virtualEnvPath} && cd {_scrapyProjectPath} && python --version",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (var process = new Process { StartInfo = activateProcessStartInfo })
            {
                process.Start();
                process.WaitForExit();
            }

            Console.WriteLine("Ejecutando los spiders de Scrapy...");
            var spiders = new List<string> { "bookspider", "techCrunch", "angelList" };
            foreach (var spiderName in spiders)
            {
                Console.WriteLine($"Ejecutando el spider: {spiderName}");
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {_virtualEnvPath} && cd {_scrapyProjectPath} && {_executeSpiderPath}scrapy.exe crawl {spiderName}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = _scrapyProjectPath
                };
                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    process.WaitForExit();
                }
            }

            if (isCron)
            {
                TimeSpan timeUntilNextRun = CalculateTimeUntilNextRun(_cronExpression);
                _timer.Change(timeUntilNextRun, Timeout.InfiniteTimeSpan);
            }            
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
