using System.Text.Json.Serialization;

namespace ShipStation.NetSdk;

public class Dimensions
{
    public Dimensions(double length, double width, double height, string units)
    {
        Length = length;
        Width = width;
        Height = height;
        Units = units;
    }

    [JsonPropertyName("length")] public double Length { get; set; }
    [JsonPropertyName("width")] public double Width { get; set; }
    [JsonPropertyName("height")] public double Height { get; set; }
    /// <summary>
    /// Units of measurement. Allowed units are: "inches", or "centimeters"
    /// </summary>
    [JsonPropertyName("units")] public string Units { get; set; }

}