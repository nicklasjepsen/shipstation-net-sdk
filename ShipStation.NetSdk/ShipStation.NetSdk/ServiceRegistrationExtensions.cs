using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace ShipStation.NetSdk
{
    public static class ServiceCollectionExtensions
    {
        public static void AddShipStation(this IServiceCollection services, ShipStationOptions options)
        {
            if(options == null)
                throw new ArgumentNullException(nameof(options));
            if(string.IsNullOrEmpty(options.BaseUrl))
                throw new ArgumentException("BaseUrl must be a valid url.");
            if (string.IsNullOrEmpty(options.ApiKey))
                throw new ArgumentException("ApiKey must be set.");
            if (string.IsNullOrEmpty(options.ApiSecret))
                throw new ArgumentException("ApiSecret must be set.");

            services.AddHttpClient<ShipStationService>(client =>
            {
                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    $"{options.ApiKey}:{options.ApiSecret}".Base64Encode());
            });
        }
    }
}
