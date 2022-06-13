namespace ShipStation.NetSdk
{
    public class ShipStationOptions
    {
        public ShipStationOptions(string baseUrl, string apiKey, string apiSecret)
        {
            BaseUrl = baseUrl;
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string BaseUrl { get; set; }
    }
}
