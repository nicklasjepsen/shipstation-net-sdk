using System.Net.Http.Json;

namespace ShipStation.NetSdk
{
    public class ShipStationService
    {
        private readonly HttpClient _api;
        
        public ShipStationService(IHttpClientFactory httpClientFactory)
        {
            _api = httpClientFactory.CreateClient(nameof(ShipStationService));
        }

        public async Task<IList<RateResponse>> GetRates(RateRequest requestModel)
        {
            var response = await _api.PostAsJsonAsync("shipments/getrates", requestModel);
            return await response.Content.ReadFromJsonAsync<List<RateResponse>>() ?? new List<RateResponse>();
        }
    }
}