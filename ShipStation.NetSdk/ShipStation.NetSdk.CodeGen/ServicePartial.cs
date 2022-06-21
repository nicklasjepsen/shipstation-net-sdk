using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStation.NetSdk.CodeGen
{
    public partial class Service
    {
        private readonly List<ScrapeResult> _scrapeResults;

        public Service(List<ScrapeResult> scrapeResults)
        {
            _scrapeResults = scrapeResults;
        }
    }
}
