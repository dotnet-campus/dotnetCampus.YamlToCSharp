using System.Collections.Generic;

namespace dotnetCampus.YamlToCSharp
{
    class CSharpClass : ICSharpNode
    {
        public List<ICSharpNode> CSharpNodeList { get; } = new List<ICSharpNode>();

        public string Value { get; set; }

        public override string ToString()
        {
            return Value + " " + CSharpNodeList.Count;
        }
    }
}