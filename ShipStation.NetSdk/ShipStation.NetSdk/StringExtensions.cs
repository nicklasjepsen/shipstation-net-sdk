using System.Text;

namespace ShipStation.NetSdk
{
    public static class StringExtensions
    {
        public static string Base64Encode(this string input)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
