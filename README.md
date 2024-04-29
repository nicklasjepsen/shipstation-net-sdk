# ShipStation SDK for .NET
.NET SDK for the ShipStation's REST API

## Getting started
### 1. Setup a new .net project
Create a console app.

Install Nugets 
```ps  
NuGet\Install-Package ShipStation.NetSdk -Version 1.1.1-beta
NuGet\Install-Package Microsoft.Extensions.Configuration.UserSecrets
NuGet\Install-Package Microsoft.Extensions.Hosting
```
Add user secrets
```ps  
dotnet user-secrets set "ShipStation:BaseUrl" "https://ssapi.shipstation.com/"
dotnet user-secrets set "ShipStation:ApiKey" ""
dotnet user-secrets set "ShipStation:ApiSecret" ""
```
Replace the contents of `Program.cs` with the following code:

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShipStation.NetSdk;

var builder = Host.CreateApplicationBuilder(args);
IConfiguration configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
builder.Services.AddShipStation(configuration);
using var host = builder.Build();

var client = host.Services.GetRequiredService<IShipStationClient>();
var request = new GetRatesRequest
{
    CarrierCode = "fedex",
    FromPostalCode = "78703", 
    Weight = new Weight { Value = 3, Units = "ounces" },
    ToCountry = "US",
    ToPostalCode = "20500",
    Dimensions = new Dimensions
    {
        Units = "lbs",
        Height = 17,
        Length = 17,
        Width = 6,
    },
};

var rates = await client.GetRatesAsync(request);
foreach (var rate in rates)
{
    Console.WriteLine($"{rate.ServiceName} - {rate.ServiceCode} - {rate.ShipmentCost}");
}

await host.RunAsync();

```

## Generating the ShipStation client code
- Using this [Postman collection](https://www.postman.com/lunar-comet-57889/workspace/shipstation/documentation/14323608-324dcda9-cc54-4359-9ef5-15f8a3537caa) 
- Export collection.
- Upload to https://app.apimatic.io/
- Go to the dashboard, something like this: https://app.apimatic.io/api-docs-preview/dashboard/662b68329f665c328341a719/v/1_0#/http/step-by-step-tutorial
- Click API Spec (upper left corner) > Open API, select YAML
- Due to problems with NSwag generation, we need to modify the YAML. Find/replace: allOf, replace with oneOf
- NSwag, use the YAML to generate the C# classes