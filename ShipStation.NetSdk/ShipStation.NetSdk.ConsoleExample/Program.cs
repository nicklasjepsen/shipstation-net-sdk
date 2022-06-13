using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ShipStation.NetSdk.ConsoleExample
{
    internal class Program
    {
        protected Program(){}

        private static async Task<int> Main()
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
                var rates = await shipStation.GetRates(new RateRequest("fedex", "78703", new Weight(3, "ounces"),
                    "US", "20500"));
                foreach (var rate in rates)
                {
                    Console.WriteLine($"ServiceName: {rate.ServiceName}, serviceCode: {rate.ServiceCode}, shipment cost: {rate.ShipmentCost}, other cost: {rate.OtherCost}");
                }
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