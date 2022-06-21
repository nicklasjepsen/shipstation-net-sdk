using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShipStation.NetSdk.CodeGen
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            //var html = new HtmlDocument();
            //html.LoadHtml(await File.ReadAllTextAsync("HTMLPage1.html"));
            
            
            var importMethod = ImportMethod.Json;
            //var c = new ModelClass("ModelOne", new List<PropertyResult>() { new PropertyResult
            //{
            //    DataTypeName = "string",
            //    IsRequired = true,
            //    Name = "requiredString"
            //},
            //    new PropertyResult
            //    {
            //        DataTypeName = "int",
            //        IsRequired = false,
            //        Name = "optionalInt"
            //    }
            //});
            //File.WriteAllText("Model.cs", c.TransformText());

            var ignoreListPath = "links-to-ignore.txt";
            if (!File.Exists(ignoreListPath))
            {
                var sr = File.CreateText(ignoreListPath);
                await sr.DisposeAsync();
            }

            var links = await WebScraper.GetAllEndpoints();

            var ignoreList = (await File.ReadAllLinesAsync(ignoreListPath)).ToList();
            var scrapeResults = new List<ScrapeResult>();
            foreach (var link in links)
            {
                if (string.IsNullOrEmpty(link) || ignoreList.Contains(link)) continue;
                Console.WriteLine(link);
                var name = new MethodName(link);
                ScrapeResult result = null;
                if (importMethod == ImportMethod.Json)
                {
                    if (File.Exists($"JsonFiles\\{name.JsonFileName}"))
                        result = JsonConvert.DeserializeObject<ScrapeResult>(File.ReadAllText($"JsonFiles\\{name.JsonFileName}"));
                }
                if (importMethod == ImportMethod.Web || result == null || !result.Properties.Any())
                {
                    result = await WebScraper.GetDocumentation(link, name);
                    if (result == null)
                    {
                        ignoreList.Add(link);
                        await File.AppendAllLinesAsync(ignoreListPath, new[] { link });
                        continue;
                    }
                    else
                    {
                        var fi = new FileInfo($"JsonFiles\\{name.JsonFileName}");
                        fi.Directory.Create();
                        await File.WriteAllTextAsync($"JsonFiles\\{name.JsonFileName}", JsonConvert.SerializeObject(result));
                    }

                }
                scrapeResults.Add(result);
                var modelClass = new ModelClass(name.ModelClassName, result.Properties);
                var classText = modelClass.TransformText();
                var path = $"Models\\{name.ModelCSharpFileName}";
                var file = new FileInfo(path);
                file.Directory.Create();
                await File.WriteAllTextAsync(path, classText);

                if (result.ReturnTypeProperties == null || !result.ReturnTypeProperties.Any())
                {
                    result.MethodName.VoidReturnType = true;
                }
                else
                {
                    var returnClass = new ModelClass(name.ReturnClassName, result.ReturnTypeProperties);
                    var returnPath = $"Models\\{name.ReturnCSharpFileName}";
                    var returnFile = new FileInfo(returnPath);
                    returnFile .Directory.Create();
                    await File.WriteAllTextAsync(returnPath, returnClass.TransformText());
                }

               
            }

            var serviceClass = new Service(scrapeResults);
            var ssPath = $"Models\\ShipStationService.cs";
            var ssFi = new FileInfo(ssPath);
            ssFi.Directory.Create();
            await File.WriteAllTextAsync(ssPath, serviceClass.TransformText());
        }
    }
}