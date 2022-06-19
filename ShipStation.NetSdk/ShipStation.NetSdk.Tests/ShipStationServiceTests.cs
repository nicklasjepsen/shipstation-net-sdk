

using System.Net.Http.Json;

namespace ShipStation.NetSdk.Tests
{
    [ExcludeFromCodeCoverage]
    public class ShipStationServiceTests : IClassFixture<ShipStationServiceFixture>
    {
        private readonly ShipStationServiceFixture _fixture;

        public ShipStationServiceTests(ShipStationServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Throws_Invalid_Arguments()
        {
            var service = _fixture.ShipStationService;
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetRates(null));
        }

        [Fact]
        public async Task GetRates_Returns_Json()
        {
            var rateRequest = new RateRequest("", "", new Weight(0, ""), "", "");
            _fixture.HandlerMock
                   .Protected()
                   .Setup<Task<HttpResponseMessage>>(
                       "SendAsync",
                       ItExpr.Is<HttpRequestMessage>(rm =>
                           rm.RequestUri.AbsoluteUri.Equals($"{_fixture.BaseUrl}{ShipStationService.Endpoint.ShipmentsGetRates}")),
               ItExpr.IsAny<CancellationToken>()
                   )
                   .ReturnsAsync(new HttpResponseMessage
                   {
                       StatusCode = HttpStatusCode.OK,
                       Content = new StringContent("[]")
                   })
                   .Verifiable();




            var service = _fixture.ShipStationService;
            var result = await service.GetRates(rateRequest);
            Assert.Equal(0, result.Count);
            _fixture.HandlerMock.Verify();
        }

        [Fact]
        public void RateRequest_Initialization()
        {
            const int weightVal = 10;
            const string units = "ounces";
            const string carrierCode = "carrierCode";
            var w = new Weight(weightVal, units);
            const string fromPostalCode = "123456";
            const string toCountry = "US";
            const string toPostalCode = "123457";
            var rr = new RateRequest(carrierCode, fromPostalCode, w, toCountry, toPostalCode);
            var confirmation = "confirm";
            rr.Confirmation = confirmation;
            var toCity = "city";
            rr.ToCity = toCity;
            const double length = 100D;
            const double width = 50D;
            const double height = 20D;
            const string unit = "unit";
            var dimensions = new Dimensions(length, width, height, unit);
            rr.Dimensions = dimensions;
            const string packageCode = "pkgcode";
            rr.PackageCode = packageCode;
            const bool residential = true;
            rr.Residential = residential;
            const string serviceCode = "servCode";
            rr.ServiceCode = serviceCode;
            Assert.Equal(weightVal, rr.Weight.Value);
            Assert.Equal(units, w.Units);
            Assert.Equal(fromPostalCode, rr.FromPostalCode);
            Assert.Equal(carrierCode, rr.CarrierCode);
            Assert.Equal(toCountry, rr.ToCountry);
            Assert.Equal(toPostalCode, rr.ToPostalCode);
            Assert.Equal(dimensions.Units, rr.Dimensions.Units);
            Assert.Equal(dimensions.Height, rr.Dimensions.Height);
            Assert.Equal(dimensions.Length, rr.Dimensions.Length);
            Assert.Equal(dimensions.Width, rr.Dimensions.Width);
            Assert.Equal(width, rr.Dimensions.Width);
            Assert.Equal(packageCode, rr.PackageCode);
            Assert.Equal(confirmation, rr.Confirmation);
            Assert.Equal(toCity, rr.ToCity);
            Assert.Equal(residential, rr.Residential);
        }
    }
}
