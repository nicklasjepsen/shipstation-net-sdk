namespace ShipStation.NetSdk
{
    public partial class ShipStationClient
    {
        internal string? AuthorizationHeader => _httpClient.DefaultRequestHeaders.Authorization?.ToString();
        internal HttpClient HttpClient => _httpClient;
    }
}
