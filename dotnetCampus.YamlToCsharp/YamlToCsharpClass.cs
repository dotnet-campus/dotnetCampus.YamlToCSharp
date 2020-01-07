using YamlDotNet.RepresentationModel;

namespace dotnetCampus.YamlToCsharp
{
    class YamlToCsharpClass
    {
        public CsharpClass ParseToCsharpClass(YamlStream yamlStream)
        {
            var mapping =
                (YamlMappingNode)yamlStream.Documents[0].RootNode;

            var csharpClass = new CsharpClass();

            ParseToCsharp(mapping, csharpClass);

            return csharpClass;
        }

        private void ParseToCsharp(YamlMappingNode mapping, CsharpClass csharpClass)
        {
            foreach (var temp in mapping.Children)
            {
                var key = temp.Key;
                var value = temp.Value;

                if (value.NodeType == YamlNodeType.Scalar)
                {
                    csharpClass.CsharpNodeList.Add(new CsharpProperty()
                    {
                        Key = key.ToString(),
                        Value = value.ToString()
                    });
                }
                else if (value.NodeType == YamlNodeType.Mapping)
                {
                    var innerClass = new CsharpClass();

                    csharpClass.CsharpNodeList.Add(innerClass);

                    innerClass.Value = key.ToString();

                    ParseToCsharp((YamlMappingNode)value, innerClass);
                }
                else
                {
                }
            }
        }

    }
}