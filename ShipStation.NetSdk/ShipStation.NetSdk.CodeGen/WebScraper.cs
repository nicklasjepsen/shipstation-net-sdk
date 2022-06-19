using HtmlAgilityPack;

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

        public static async Task<ScrapeResult?> GetDocumentation(string url)
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
                var scrapeResult = new ScrapeResult
                {
                    DisplayName = doc.DocumentNode.SelectNodes("//h1").First().InnerText,
                    Properties = new List<PropertyResult>(),
                    Name = url,
                    Url = $"{ApiUrl}{url}"
                };

                foreach (var row in doc.DocumentNode.SelectNodes("//table//tr"))
                {
                    var cells = row.SelectNodes("td");
                    if(cells == null || cells.Count < 3)
                        continue;
                    scrapeResult.Properties.Add(new PropertyResult
                    {
                        Name = cells[0]?.InnerText,
                        DataTypeName = cells[1]?.InnerText.Split(',').FirstOrDefault()?.Trim(),
                        IsRequired = cells[1]?.InnerText.Split(',').Last().Trim().ToLower() == "required",
                        Description = cells[2]?.InnerText,
                    });
                        
                }
                

                return scrapeResult;
            }
            return null;

        }
    }
}
