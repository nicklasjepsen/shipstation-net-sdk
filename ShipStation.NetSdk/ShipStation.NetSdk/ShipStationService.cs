using System.Net.Http.Json;

namespace ShipStation.NetSdk
{
    public class ShipStationService
    {
        public static class Endpoint
        {
            public const string ShipmentsGetRates = "shipments/getrates";
        }

        private readonly HttpClient _api;

        public ShipStationService(HttpClient api)
        {
            _api = api;
        }

        public async Task<IList<RateResponse>> GetRates(RateRequest requestModel)
        {
            if (requestModel == null) throw new ArgumentNullException(nameof(requestModel));
            var response = await _api.PostAsJsonAsync(Endpoint.ShipmentsGetRates, requestModel);
            return await response.Content.ReadFromJsonAsync<List<RateResponse>>() ?? new List<RateResponse>();
        }
    }
}