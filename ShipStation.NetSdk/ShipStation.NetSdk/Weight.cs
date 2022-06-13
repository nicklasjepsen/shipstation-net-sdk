using System.Text.Json.Serialization;

namespace ShipStation.NetSdk;

public class Weight
{
    public Weight(double value, string units)
    {
        Value = value;
        Units = units;
    }

    [JsonPropertyName("value")] public double Value { get; set; }
    /// <summary>
    /// Allowed units are: "pounds", "ounces", or "grams"
    /// </summary>
    [JsonPropertyName("units")] public string Units{ get; set; }
}