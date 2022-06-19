using Newtonsoft.Json;

namespace ShipStation.NetSdk.CodeGen
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var c = new ModelClass("ModelOne", new List<PropertyResult>() { new PropertyResult
            {
                DataTypeName = "string",
                IsRequired = true,
                Name = "requiredString"
            },
                new PropertyResult
                {
                    DataTypeName = "int",
                    IsRequired = false,
                    Name = "optionalInt"
                }
            });
            File.WriteAllText("Model.cs", c.TransformText());

            var ignoreListPath = "links-to-ignore.txt";
            if (!File.Exists(ignoreListPath))
            {
                var sr = File.CreateText(ignoreListPath);
                await sr.DisposeAsync();
            }
            
            var links = await WebScraper.GetAllEndpoints();

            var ignoreList = (await File.ReadAllLinesAsync(ignoreListPath)).ToList();

            foreach (var link in links)
            {
                if (string.IsNullOrEmpty(link) || ignoreList.Contains(link)) continue;
                Console.WriteLine(link);
                var results = await WebScraper.GetDocumentation(link);
                if (results == null)
                {
                    ignoreList.Add(link);
                    await File.AppendAllLinesAsync(ignoreListPath, new[] { link });
                }

                var modelClass = new ModelClass(results.ClassName, results.Properties);
                var classText = modelClass.TransformText();
                var path = $"Models\\{results.ClassName}.cs";
                var file = new FileInfo(path);
                file.Directory.Create(); 
                await File.WriteAllTextAsync(path, classText);
            }
        }
    }
}