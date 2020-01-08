﻿using System.IO;
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

            var str = $@"//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace {classNamespace}
{{
    [System.CodeDom.Compiler.GeneratedCode(""dotnetCampus.YamlToCsharp"", ""1.0.0"")]
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