using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using NCrontab;
using Serilog;

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

            // Load cron expression and paths from configuration
            _cronExpression = _configuration["CronJob:CronExpression"];
            _virtualEnvPath = _configuration["PythonSettings:VirtualEnvPath"];
            _scrapyProjectPath = _configuration["PythonSettings:ScrapyProjectPath"];
            _executeSpiderPath = _configuration["PythonSettings:ExecuteSpiderPath"];

            if (string.IsNullOrEmpty(_cronExpression))
            {
                Log.Error("No valid cron expression found in the configuration.");
                return;
            }

#if !TESTING
            // In normal mode, setup the cron job
            InitializeCronJob();
#endif
        }

        private void InitializeCronJob()
        {
            try
            {
                TimeSpan timeUntilNextRun = CalculateTimeUntilNextRun(_cronExpression);
                _timer = new Timer(async _ => await ExecuteTaskAsync(), null, timeUntilNextRun, Timeout.InfiniteTimeSpan);
                Log.Information("Cron job initialized with next run scheduled in {TimeUntilNextRun} minutes.", timeUntilNextRun.TotalMinutes);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to initialize cron job.");
            }
        }

        public async Task StartAsync()
        {
            try
            {
                Log.Information("Cron job Program started.");
                // Additional startup logic if needed
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to start cron job.");
            }
        }

        public async Task ExecuteOnce()
        {
            try
            {
                await ExecuteTaskAsync(false);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error executing the task manually.");
            }
        }

        private async Task ExecuteTaskAsync(bool isCron = true)
        {
            try
            {
                Log.Information("Activating virtual environment and changing to project directory...");

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

                Log.Information("Executing Scrapy spiders...");

                var spiders = new List<string> { "bookspider", "techCrunch", "angelList" };
                foreach (var spiderName in spiders)
                {
                    Log.Information("Executing spider: {SpiderName}", spiderName);
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
                    Log.Information("Task execution completed. Next run scheduled in {TimeUntilNextRun} minutes.", timeUntilNextRun.TotalMinutes);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error executing the task.");
            }
        }

        private TimeSpan CalculateTimeUntilNextRun(string cronExpression)
        {
            try
            {
                var cronSchedule = CrontabSchedule.Parse(cronExpression);
                DateTime now = DateTime.Now;
                DateTime nextRun = cronSchedule.GetNextOccurrence(now);
                Log.Information("Next cron execution scheduled at {NextRun}.", nextRun);
                return nextRun - now;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to calculate time until next run.");
                return TimeSpan.Zero;
            }
        }
    }
}
