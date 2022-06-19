using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace ShipStation.NetSdk.CodeGen
{
    internal class ScrapeResult
    {
        public string Url { get; set; }

        public string ClassName
        {
            get
            {
                return Regex.Replace(string.Join("",
                    Url.Replace("https://ssapi.shipstation.com/docs/api/", "").Split('/')
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => x.FirstCharToUpper())), @"[^a-zA-Z0-9]+", string.Empty) ;
            }
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IList<PropertyResult> Properties { get; set; }

        public override string ToString()
        {
            return
                $"Display name: {DisplayName}{Environment.NewLine}Url: {Url}{Environment.NewLine}Name: {Name}{Environment.NewLine}Properties: {Properties.Count}";
        }
    }
}
