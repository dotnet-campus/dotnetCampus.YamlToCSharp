using System.Text;
using YamlDotNet.RepresentationModel;

namespace dotnetCampus.YamlToCSharp
{
    /// <summary>
    /// 将 Yaml 转换为 C# 字典
    /// </summary>
    public class YamlToCSharpDictionary
    {
        public string ValuePrefix = "Lang.";

        public string ParseToCSharp(YamlStream yamlStream)
        {
            var yamlToCSharpClass = new YamlToCSharpClass();
            var csharpClass = yamlToCSharpClass.ParseToCSharpClass(yamlStream);

            return ParseCSharpClassToDictionary(csharpClass);
        }

        internal string ParseCSharpClassToDictionary(CSharpClass csharpClass)
        {
            var prefix = ValuePrefix;
            var str = new StringBuilder();

            foreach (var csharpNode in csharpClass.CSharpNodeList)
            {
                Parse(csharpNode, prefix, str);
            }

            return $@"           new Dictionary<string, string>()
           {{
               {str}
           }}";
        }

        private void Parse(ICSharpNode csharpNode, string prefix, StringBuilder str)
        {
            if (csharpNode is CSharpClass csharpClass)
            {
                prefix += $"{csharpClass.Value}.";
                foreach (var temp in csharpClass.CSharpNodeList)
                {
                    Parse(temp, prefix, str);
                }
            }
            else if (csharpNode is CSharpProperty csharpProperty)
            {
                var value = csharpProperty.Value;

                value = value.Replace("\"", "\"\"");

                str.Append($"{{ \"{prefix}{csharpProperty.Key}\",@\"{value}\" }},\r\n");
            }
        }
    }
}