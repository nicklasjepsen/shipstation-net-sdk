using System.Text.RegularExpressions;

namespace ShipStation.NetSdk.CodeGen
{
    public class ScrapeResult
    {
        public ScrapeResult(MethodName methodName)
        {
            MethodName = methodName;
        }
        
        public string Url { get; set; }
        public MethodName MethodName { get; }

        //private string GetComputerFriendlyName
        //{
        //    get
        //    {
        //        return Regex.Replace(string.Join("",
        //            Url.Replace("/docs/api/", "").Split('/')
        //                .Where(x => !string.IsNullOrEmpty(x))
        //                .Select(x => x.FirstCharToUpper())), @"[^a-zA-Z0-9]+", string.Empty) ;
        //    }
        //}
        
        public string DisplayName { get; set; }
        public IList<PropertyResult> Properties { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public IList<PropertyResult> ReturnTypeProperties { get; set; }  

        public override string ToString()
        {
            return
                $"Display name: {DisplayName}{Environment.NewLine}Url: {Url}{Environment.NewLine}Name: {MethodName}{Environment.NewLine}Properties: {Properties.Count}";
        }
    }
}
