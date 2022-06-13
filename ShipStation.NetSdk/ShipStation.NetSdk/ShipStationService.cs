using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ShipStation.NetSdk
{
    public class ShipStationService
    {
        private readonly HttpClient _api;
        
        public ShipStationService(IHttpClientFactory httpClientFactory, ShipStationOptions options)
        {
            _api = httpClientFactory.CreateClient("shipstation");
            _api.BaseAddress = new Uri(options.BaseUrl);
            _api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", $"{options.ApiKey}:{options.ApiSecret}".Base64Encode());
        }

        public async Task<IList<RateResponse>> GetRates(RateRequest requestModel)
        {
            var response = await _api.PostAsJsonAsync("shipments/getrates", requestModel);
            return await response.Content.ReadFromJsonAsync<List<RateResponse>>() ?? new List<RateResponse>();
        }
    }
}