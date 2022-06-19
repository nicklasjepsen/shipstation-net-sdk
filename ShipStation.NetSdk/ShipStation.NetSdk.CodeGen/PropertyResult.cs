using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStation.NetSdk.CodeGen
{
    public class PropertyResult
    {
        public string Name { get; set; }
        public string DataTypeName { get; set; }
        public bool IsRequired { get; set; }
        public string Description { get; set; }
    }
}
