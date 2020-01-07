using System.Collections.Generic;

namespace dotnetCampus.YamlToCsharp
{
    class CsharpClass : ICsharpNode
    {
        public List<ICsharpNode> CsharpNodeList { get; } = new List<ICsharpNode>();

        public string Value { get; set; }

        public override string ToString()
        {
            return Value + " " + CsharpNodeList.Count;
        }
    }
}