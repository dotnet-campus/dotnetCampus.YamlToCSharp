using System.Text;
using YamlDotNet.RepresentationModel;

namespace dotnetCampus.YamlToCsharp
{
    /// <summary>
    /// 将 Yaml 转换为 C# 字典
    /// </summary>
    public class YamlToCsharpDictionary
    {
        public string ValuePrefix = "Lang.";

        public string ParseToCsharp(YamlStream yamlStream)
        {
            var yamlToCsharpClass = new YamlToCsharpClass();
            var csharpClass = yamlToCsharpClass.ParseToCsharpClass(yamlStream);

            return ParseCsharpClassToDictionary(csharpClass);
        }

        internal string ParseCsharpClassToDictionary(CsharpClass csharpClass)
        {
            var prefix = ValuePrefix;
            var str = new StringBuilder();

            foreach (var csharpNode in csharpClass.CsharpNodeList)
            {
                Parse(csharpNode, prefix, str);
            }

            return $@"           new Dictionary<string, string>()
           {{
               {str}
           }}";
        }

        private void Parse(ICsharpNode csharpNode, string prefix, StringBuilder str)
        {
            if (csharpNode is CsharpClass csharpClass)
            {
                prefix += $"{csharpClass.Value}.";
                foreach (var temp in csharpClass.CsharpNodeList)
                {
                    Parse(temp, prefix, str);
                }
            }
            else if (csharpNode is CsharpProperty csharpProperty)
            {
                var value = csharpProperty.Value;

                value = value.Replace("\"", "\"\"");

                str.Append($"{{ \"{prefix}{csharpProperty.Key}\",@\"{value}\" }},\r\n");
            }
        }
    }
}