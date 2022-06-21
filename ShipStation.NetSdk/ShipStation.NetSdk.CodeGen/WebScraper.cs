using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace ShipStation.NetSdk.CodeGen
{
    internal static class WebScraper
    {
        private const string ApiUrl = $"https://ssapi.shipstation.com/";
        private const string DocUrl = $"https://www.shipstation.com/";

        public static async Task<IEnumerable<string?>> GetAllEndpoints()
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync($"{DocUrl}docs/api/");
            var links = doc.DocumentNode
                .SelectNodes("//ul[@class='" + "page-list" + "']").First()
                .SelectNodes("//a").Select(x => x.Attributes["href"]?.Value);

            return links;
        }

        public static async Task<ScrapeResult?> GetDocumentation(string url, MethodName methodName)
        {
            if (url.StartsWith(@"/"))
                url = url[1..];
            if (Uri.TryCreate($"{DocUrl}{url}", UriKind.Absolute, out var myUri))
            {
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(myUri.ToString());
                var heading = doc.DocumentNode.SelectNodes("//h1")?.FirstOrDefault();
                var table = doc.DocumentNode.SelectNodes("//table")?.FirstOrDefault();
                if (table == null)
                    return null;
                var valid = table.SelectNodes("//th")?.Select(x => x.InnerText)
                    .SequenceEqual(new List<string>() { "Name", "Data Type", "Description" });
                if (heading == null || valid is null or false)
                {
                    return null;
                }

                var methodAndName = doc.DocumentNode
                    .Descendants(0)
                    .Where(n => n.HasClass("language-http") && n.HasClass("line-numbers")).FirstOrDefault()?.InnerText
                    .Split(" ");
                if (methodAndName == null || methodAndName.Length < 2)
                    return null;

                var httpMethod = methodAndName.First();
                if (httpMethod == null)
                    return null;

                var scrapeResult = new ScrapeResult(methodName)
                {
                    DisplayName = doc.DocumentNode.SelectNodes("//h1").First().InnerText,
                    Properties = new List<PropertyResult>(),
                    ReturnTypeProperties = new List<PropertyResult>(),
                    Url = url,
                    HttpMethod = new HttpMethod(httpMethod)
                };

                foreach (var row in doc.DocumentNode.SelectNodes("//table//tr"))
                {
                    var cells = row.SelectNodes("td");
                    if (cells == null || cells.Count < 3)
                        continue;
                    scrapeResult.Properties.Add(new PropertyResult
                    {
                        Name = cells[0]?.InnerText,
                        DataTypeName = cells[1]?.InnerText.Split(',').FirstOrDefault()?.Trim(),
                        IsRequired = cells[1]?.InnerText.Split(',').Last().Trim().ToLower() == "required",
                        Description = cells[2]?.InnerText,
                    });

                }

                var requestResponseHeaderNodes = doc.DocumentNode.Descendants().Where(x =>
                    x.Name == "h2" && x.InnerText.ToLower().Contains("Example Request".ToLower())).ToList();

                if (requestResponseHeaderNodes.Count == 0)
                {
                    return scrapeResult;
                }
                string json = string.Empty;
                // There's a bug in the ShipStation doc - in same pages they have 2 "Example Request" and no "Example Response.
                // Therefore we assume that the second "Example Request" is in fact the "Example Response".
                if (requestResponseHeaderNodes.Count > 1)
                {
                    json = doc.DocumentNode.SelectSingleNode("(//a[contains(@href,'#example-request')])[2]/following::div")
                        .Descendants()
                        .Where(x => x.HasClass("language-json"))
                        .FirstOrDefault()
                        .InnerText;
                }
                else if (requestResponseHeaderNodes.Count == 1)
                {
                    json = doc.DocumentNode.SelectSingleNode("//a[contains(@href,'#example-response')]/following::div")?
                        .Descendants()?.Where(x => x.HasClass("language-json"))?
                        .FirstOrDefault()?
                        .InnerText;
                }

                if (!string.IsNullOrEmpty(json))
                {
                    var token = JToken.Parse(json);
                    JToken sampleToken = null;
                    if (token is JArray)
                    {
                        sampleToken = token.First;
                    }
                    else
                    {
                        sampleToken = token;
                    }

                    foreach (JProperty jt in sampleToken)
                    {
                        scrapeResult.ReturnTypeProperties.Add(new PropertyResult
                        {
                            DataTypeName = jt.First.Type.ToString().ToLower(),
                            Name = jt.Name
                        });
                    }
                }
                return scrapeResult;
            }
            return null;

        }
    }
}
