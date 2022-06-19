namespace ShipStation.NetSdk.Tests;

[ExcludeFromCodeCoverage]
public class ShipStationServiceFixture : IDisposable
{
    public string BaseUrl = "https://ssapi.shipstation.com/";
    private readonly MockRepository _mockRepository;
    public Mock<HttpMessageHandler> HandlerMock { get; }
    public HttpClient? MagicHttpClient { get; set; }

    public ShipStationService ShipStationService
    {
        get
        {
            MagicHttpClient = new HttpClient(HandlerMock.Object);
            MagicHttpClient.BaseAddress = new Uri(BaseUrl);
            return new ShipStationService(MagicHttpClient);
        }
    }

    public ShipStationServiceFixture()
    {
        _mockRepository = new MockRepository(MockBehavior.Default);
        HandlerMock = _mockRepository.Create<HttpMessageHandler>();
    }

    public void Dispose()
    {
        MagicHttpClient?.Dispose();
    }

}