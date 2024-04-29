namespace ShipStation.NetSdk.Tests
{
    [ExcludeFromCodeCoverage]
    public class ServiceRegistrationTests
    {
        [Fact]
        public void ServiceCollectionRegistration_Options_Null_Exception()
        {
            IServiceCollection services = new ServiceCollection();
            Assert.Throws<ArgumentNullException>(() => services.AddShipStation(null));
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("https://valid.url/", "", "")]
        [InlineData("https://valid.url/", "validapikey", "")]
        public void ServiceCollectionRegistration_Options_Invalid_Values(string url, string apiKey, string apiSecret)
        {
            var services = new ServiceCollection();
            Assert.Throws<ArgumentException>(() => services.AddShipStation(new ShipStationOptions(url, apiKey, apiSecret)));
        }

        [Theory]
        [InlineData("https://valid.url/", "validapikey", "validapisecret")]
        public void ServiceCollectionRegistration_Options_Valid_Values(string url, string apiKey, string apiSecret)
        {
            var services = new ServiceCollection();
            services.AddShipStation(new ShipStationOptions(url, apiKey, apiSecret));
        }

        [Theory]
        [InlineData("https://valid.url/", "validapikey", "validapisecret")]
        public void ServiceCollectionRegistration_Options_Assigned_In_Service(string url, string apiKey, string apiSecret)
        {
            var services = new ServiceCollection();
            services.AddShipStation(new ShipStationOptions(url, apiKey, apiSecret));

            var provider = services.BuildServiceProvider();

            var shipStationClient = (ShipStationClient)provider.GetRequiredService<IShipStationClient>();
            Assert.NotNull(shipStationClient);
            Assert.Equal(url, shipStationClient.HttpClient.BaseAddress.ToString());
            Assert.Equal("Basic " + $"{apiKey}:{apiSecret}".Base64Encode(), shipStationClient.AuthorizationHeader);
        }
    }
}