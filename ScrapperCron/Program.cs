using Microsoft.Extensions.Hosting;
using ScrapperCron.Infrastructure;
using Serilog;

namespace ScrapperCron
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Startup.ConfigureLogging();             
                var host = Startup.CreateHostBuilder(args).Build();
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The application failed to start correctly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
