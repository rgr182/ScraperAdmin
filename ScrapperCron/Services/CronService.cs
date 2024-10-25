using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using NCrontab;
using System;

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
        private readonly IMongoCollection<BsonDocument> _mongoCollection;

        public CronService(IConfiguration configuration)
        {            
            _configuration = configuration;

            // Retrieve MongoDB settings from the appsettings.cron.json.
            var mongoConnectionString = _configuration["MongoDB:ConnectionString"];
            var mongoDatabaseName = _configuration["MongoDB:DatabaseName"];
            var mongoCollectionName = _configuration["MongoDB:Collections:RawHtmlCollectionName"];

            // Configuración de la conexión a MongoDB
            var mongoClient = new MongoClient(mongoConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);
            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>(mongoCollectionName);

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
            using (var activateProcess = new Process { StartInfo = activateProcessStartInfo })
            {
                activateProcess.Start();
                activateProcess.WaitForExit();
            }
            Console.WriteLine("Ejecutando los spiders de Scrapy...");
            var spiders = new List<string> { "bookspider", "techCrunch", "angelList" };
            foreach (var spiderName in spiders)
            {
                await UpdateScraperStatusAsync(spiderName, "Scraping en Proceso");

                Console.WriteLine($"Ejecutando el spider: {spiderName}");
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {_virtualEnvPath} && cd {_scrapyProjectPath} && {_executeSpiderPath}scrapy.exe crawl {spiderName}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = _scrapyProjectPath
                };
                using (Process process = new Process { StartInfo = processStartInfo })
                {
                    try
                    {
                        process.Start();
                        process.WaitForExit();
                        if (!process.HasExited)
                        {
                            process.Kill();
                        }
                        if (process.ExitCode == 0)
                        {
                            await UpdateScraperStatusAsync(spiderName, "Scraping Completado");
                        }
                        else
                        {
                            await UpdateScraperStatusAsync(spiderName, "Scraping Fallido");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ocurrió un error al ejecutar el spider {spiderName}: {ex.Message}");
                        await UpdateScraperStatusAsync(spiderName, "Scraping Fallido");
                    }
                }
            }
            if (isCron)
            {
                TimeSpan timeUntilNextRun = CalculateTimeUntilNextRun(_cronExpression);
                _timer.Change(timeUntilNextRun, Timeout.InfiniteTimeSpan);
            }
        }
        private async Task UpdateScraperStatusAsync(string title, string status)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("title", title);
            var update = Builders<BsonDocument>.Update
                .Set("status", status)
                .Set("lastExecutionDate", DateTime.Now);
            var result = await _mongoCollection.UpdateOneAsync(filter, update);
            if (result.ModifiedCount > 0)
            {
                Console.WriteLine($"Documento '{title}' actualizado con estado: {status}");
            }
            else
            {
                Console.WriteLine($"No se encontró el documento con el título '{title}' para actualizar.");
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
