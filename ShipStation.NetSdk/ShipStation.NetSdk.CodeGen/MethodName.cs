using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStation.NetSdk.CodeGen
{
    public class MethodName
    {
        public MethodName(string rawName)
        {
            RawName = rawName;
        }
        
        public string ModelClassName => Name + "Model";

        public string MethodReturnTypeString
        {
            get
            {
                if (VoidReturnType)
                    return "Task";
                else
                {
                    return $"Task<{ReturnClassName}>";
                }
            }
        }
        public string ReturnClassName
        {
            get
            {
                return $"{Name}Result";
            }
        }

        public bool VoidReturnType { get; set; }
        public string ModelCSharpFileName => Name + ".cs";
        public string ReturnCSharpFileName => Name + "Result" + ".cs";
        public string JsonFileName => Name + ".json";

        public string RawName { get; }
        public string Name => string.Join("", string.Join("",
                RawName.Replace("/docs/api/", "")
                    .Split("/")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.FirstCharToUpper().GetUntil('?')))
            .Split('-').Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.FirstCharToUpper()));

    }
}
