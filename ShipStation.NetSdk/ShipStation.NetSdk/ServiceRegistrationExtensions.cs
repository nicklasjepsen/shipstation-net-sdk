using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace ShipStation.NetSdk
{
    public static class ServiceCollectionExtensions
    {
        public static void AddShipStation(this IServiceCollection services, ShipStationOptions options)
        {
            services.AddHttpClient<ShipStationService>(client =>
            {
                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    $"{options.ApiKey}:{options.ApiSecret}".Base64Encode());
            });
        }
    }
}
