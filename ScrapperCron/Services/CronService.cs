using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
            Console.WriteLine("Activando entorno virtual y cambiando al directorio del proyecto...");

            // Ruta al entorno virtual de Python
            string activateVenv = @"C:\Users\GIBRAN\Python\BoilerPlateScrapy\venv\Scripts\activate.bat";

            // Directorio del proyecto de Scrapy
            string scrapyProjectPath = @"C:\Users\GIBRAN\Python\BoilerPlateScrapy\boilerplateScrapy";

            // Primero, activamos el entorno virtual y verificamos la versión de Python
            var activateProcessStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",  // Ejecuta cmd.exe
                Arguments = $"/C {activateVenv} && cd {scrapyProjectPath} && python --version",  // Verifica que estamos usando la versión correcta de Python
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = activateProcessStartInfo })
            {
                process.Start();
                process.WaitForExit();  // Asegura que el proceso se complete antes de continuar
            }

            // Ahora ejecuta cada spider por separado
            Console.WriteLine("Ejecutando los spiders de Scrapy...");

            var spiders = new List<string> { "bookspider", "techCrunch", "angelList" };

            foreach (var spiderName in spiders)
            {
                Console.WriteLine($"Ejecutando el spider: {spiderName}");

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {activateVenv} && cd {scrapyProjectPath} && C:\\Users\\GIBRAN\\Python\\BoilerPlateScrapy\\venv\\Scripts\\scrapy.exe crawl {spiderName}",  // Usa la ruta completa de scrapy
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = scrapyProjectPath  // Establece el directorio de trabajo al proyecto Scrapy
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    process.WaitForExit();  // Espera a que el proceso termine
                }
            }

            // Configura el temporizador para la siguiente ejecución según la expresión cron
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
