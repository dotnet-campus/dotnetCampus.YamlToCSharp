using YamlDotNet.RepresentationModel;

namespace dotnetCampus.YamlToCSharp
{
    class YamlToCSharpClass
    {
        public CSharpClass ParseToCSharpClass(YamlStream yamlStream)
        {
            var mapping =
                (YamlMappingNode)yamlStream.Documents[0].RootNode;

            var csharpClass = new CSharpClass();

            ParseToCSharp(mapping, csharpClass);

            return csharpClass;
        }

        private void ParseToCSharp(YamlMappingNode mapping, CSharpClass csharpClass)
        {
            foreach (var temp in mapping.Children)
            {
                var key = temp.Key;
                var value = temp.Value;

                if (value.NodeType == YamlNodeType.Scalar)
                {
                    csharpClass.CSharpNodeList.Add(new CSharpProperty()
                    {
                        Key = key.ToString(),
                        Value = value.ToString()
                    });
                }
                else if (value.NodeType == YamlNodeType.Mapping)
                {
                    var innerClass = new CSharpClass();

                    csharpClass.CSharpNodeList.Add(innerClass);

                    innerClass.Value = key.ToString();

                    ParseToCSharp((YamlMappingNode)value, innerClass);
                }
                else
                {
                }
            }
        }

    }
}