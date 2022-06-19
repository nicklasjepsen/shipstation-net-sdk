using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStation.NetSdk.CodeGen
{
    public partial class ModelClass
    {
        private readonly string _className;
        private readonly IEnumerable<PropertyResult> _props;

        public ModelClass(string className, IEnumerable<PropertyResult> props)
        {
            _className = className;
            _props = props;
        }
    }
}
