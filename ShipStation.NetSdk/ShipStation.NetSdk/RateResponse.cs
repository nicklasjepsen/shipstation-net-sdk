using System.Text.Json.Serialization;

namespace ShipStation.NetSdk
{
    public class RateResponse
    {
        [JsonPropertyName("serviceName")]
        public string? ServiceName { get; set; }
        [JsonPropertyName("serviceCode")]
        public string? ServiceCode { get; set; }
        [JsonPropertyName("shipmentCost")]
        public float ShipmentCost { get; set; }
        [JsonPropertyName("otherCost")]
        public float OtherCost { get; set; }
    }

}
