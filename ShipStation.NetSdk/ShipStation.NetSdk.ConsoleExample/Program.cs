using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ShipStation.NetSdk.ConsoleExample
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
         
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddShipStation(new ShipStationOptions(config["ShipStation:BaseUrl"], config["ShipStation:ApiKey"], config["ShipStation:ApiSecret"]));
                }).UseConsoleLifetime();

            var host = builder.Build();

            try
            {
                var shipStation = host.Services.GetRequiredService<ShipStationService>();
            }
            catch (Exception ex)
            {
                var logger = host.Services.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred.");
            }

            return 0;
        }
    }
}