using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShipStation.NetSdk.Core;

namespace ShipStation.NetSdk.ConsoleExample
{
    public class SdkTest(IShipStationClient client)
    {
        public async Task Test()
        {
            var request = new GetRatesRequest
            {
                CarrierCode = "fedex",
                FromPostalCode = "78703",
                Weight = new Weight6 { Value = 3, Units = "ounces" },
                ToCountry = "US",
                ToPostalCode = "20500",
                Dimensions = new Core.Dimensions
                {
                    Units = "lbs",
                    Height = 17,
                    Length = 17,
                    Width = 6,
                },
            };
            var rates = await client.GetRatesAsync(request);

            Console.WriteLine(string.Join(',', rates.Select(r => r.OtherCost)));
        }
    }

    internal class Program
    {
        protected Program() { }



        private static async Task<int> Main()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddShipStationCore(new ShipStationOptions(config["ShipStation:BaseUrl"], config["ShipStation:ApiKey"], config["ShipStation:ApiSecret"]));
                    //services.AddShipStation(new ShipStationOptions(config["ShipStation:BaseUrl"], config["ShipStation:ApiKey"], config["ShipStation:ApiSecret"]));

                    services.AddTransient<SdkTest>();
                }).UseConsoleLifetime();

            var host = builder.Build();

            await host.Services.GetRequiredService<SdkTest>().Test();

            return 0;
        }
    }
}