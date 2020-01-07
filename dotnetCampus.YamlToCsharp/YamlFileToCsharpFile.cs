using System.IO;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace dotnetCampus.YamlToCsharp
{
    /// <summary>
    /// 将 Yaml 文件转换为 Csharp 文件
    /// </summary>
    public class YamlFileToCsharpFile
    {
        /// <summary>
        /// 将 Yaml 文件转换为 Csharp 文件
        /// </summary>
        /// <param name="yamlFile"></param>
        /// <param name="saveCsharpFile"></param>
        /// <param name="classNamespace">类命名空间</param>
        /// <param name="interfaceName">继承的接口</param>
        /// <param name="className">类名</param>
        /// <param name="methodName"></param>
        public void ParseToCsharpFile(FileInfo yamlFile, FileInfo saveCsharpFile,
            string classNamespace = "dotnetCampus.Localizations",
            string interfaceName = "",
            string className = "",
            string methodName = "GetLang")
        {
            var yaml = new YamlStream();

            yaml.Load(new StringReader(File.ReadAllText(yamlFile.FullName)));

            var yamlToCsharpDictionary = new YamlToCsharpDictionary();
            var dictionary = yamlToCsharpDictionary.ParseToCsharp(yaml);

            if (string.IsNullOrEmpty(className))
            {
                className = Path.GetFileNameWithoutExtension(saveCsharpFile.FullName);
            }

            if (!string.IsNullOrEmpty(interfaceName))
            {
                if (!interfaceName.StartsWith(":"))
                {
                    interfaceName = $": {interfaceName}";
                }
            }

            var str = $@"
using System.Collections.Generic;

namespace {classNamespace}
{{
    public class {className} {interfaceName}
    {{
        public Dictionary<string, string> {methodName}()
        {{
            return {dictionary};
        }}
    }}
}}";

            Directory.CreateDirectory(saveCsharpFile.DirectoryName);
            File.WriteAllText(saveCsharpFile.FullName, str, Encoding.UTF8);
        }
    }
}