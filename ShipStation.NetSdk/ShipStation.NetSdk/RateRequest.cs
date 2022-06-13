using System.Text.Json.Serialization;

namespace ShipStation.NetSdk
{
    public class RateRequest
    {
        public RateRequest(string carrierCode, string serviceCode, string packageCode, string fromPostalCode, Weight weight, string toCountry, string toPostalCode)
        {
            CarrierCode = carrierCode;
            ServiceCode = serviceCode;
            PackageCode = packageCode;
            FromPostalCode = fromPostalCode;
            Weight = weight;
            ToCountry = toCountry;
            ToPostalCode = toPostalCode;
        }

        [JsonPropertyName("carrierCode")] public string CarrierCode { get; set; }
        [JsonPropertyName("serviceCode")] public string ServiceCode { get; set; }
        [JsonPropertyName("packageCode")] public string PackageCode { get; set; }
        [JsonPropertyName("fromPostalCode")] public string FromPostalCode { get; set; }
        [JsonPropertyName("weight")] public Weight Weight { get; set; }
        [JsonPropertyName("toCountry")] public string ToCountry { get; set; }
        [JsonPropertyName("toPostalCode")] public string ToPostalCode { get; set; }

        /// <summary>
        /// The type of delivery confirmation that is to be used once the shipment is created. Possible values: none, delivery, signature, adult_signature, and direct_signature. direct_signature is available for FedEx only.
        /// </summary>
        [JsonPropertyName("confirmation")] public string? Confirmation { get; set; }
        [JsonPropertyName("toState")] public string? ToState { get; set; }
        [JsonPropertyName("toCity")] public string? ToCity { get; set; }
        [JsonPropertyName("dimensions")] public Dimensions? Dimensions { get; set; }
        /// <summary>
        /// Carriers may return different rates based on if the address is commercial (false) or residential (true). Default value: false
        /// </summary>
        [JsonPropertyName("residential")] public bool? Residential { get; set; }
    }
}
