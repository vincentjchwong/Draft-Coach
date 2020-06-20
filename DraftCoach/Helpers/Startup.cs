using DraftCoach.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace DraftCoach.Helpers
{
    public class Startup
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configurations"));
                    configApp.AddJsonFile("AppConfig.json");
                    configApp.AddJsonFile("DataLocations.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Suppresses startup/shutdown messages for releases
                    if (hostContext.Configuration[Settings.Environment] == "Release")
                    {
                        services.Configure<ConsoleLifetimeOptions>(options =>
                                                options.SuppressStatusMessages = true);
                    }

                    services.AddHostedService<HostedService>();
                    services.AddTransient<IDraftService, DraftService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    if (hostingContext.Configuration[Settings.Environment] == "Release")
                    {
                        logging.AddSerilog(new LoggerConfiguration()
                            .CreateLogger());
                    }
                    else
                    {
                        logging.AddSerilog(new LoggerConfiguration()
                            .WriteTo.Console()
                            .MinimumLevel.Debug()
                            .CreateLogger());
                    }
                });

            await builder.RunConsoleAsync();
        }
    }
}
