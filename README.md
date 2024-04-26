# shipstation-net-sdk
.NET SDK for the ShipStation REST API

# Run the console app
You need to add the ShipStation api options using user secrets:

```
dotnet user-secrets set "ShipStation:BaseUrl" "https://ssapi.shipstation.com/"
dotnet user-secrets set "ShipStation:ApiKey" ""
dotnet user-secrets set "ShipStation:ApiSecret" ""
```

## Generating the ShipStation client code
- Using this [Postman collection](https://www.postman.com/lunar-comet-57889/workspace/shipstation/documentation/14323608-324dcda9-cc54-4359-9ef5-15f8a3537caa) 
- Export collection.
- Upload to https://app.apimatic.io/
- Go to the dashboard, something like this: https://app.apimatic.io/api-docs-preview/dashboard/662b68329f665c328341a719/v/1_0#/http/step-by-step-tutorial
- Click API Spec (upper left corner) > Open API, select YAML
- Due to problems with NSwag generation, we need to modify the YAML. Find/replace: allOf, replace with oneOf
- NSwag, use the YAML to generate the C# classes

