namespace ShipStation.NetSdk.Tests
{
    [ExcludeFromCodeCoverage]
    public class ServiceRegistrationTests
    {
        [Fact]
        public void ServiceCollectionRegistration_Options_Null_Exception()
        {
            var services = new ServiceCollection();
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
            var service = provider.GetRequiredService<IHttpClientFactory>();
            Assert.NotNull(service);
            var httpClient = service.CreateClient(nameof(ShipStationService));
            Assert.NotNull(httpClient);
            Assert.Equal(url, httpClient.BaseAddress?.ToString());
            Assert.Equal($"{apiKey}:{apiSecret}".Base64Encode(), httpClient.DefaultRequestHeaders.Authorization?.Parameter);
        }
    }
}