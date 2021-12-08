using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace dotnetCampus.YamlToCSharp.Utils
{
    internal static class IdentifierHelper
    {
        /// <summary>
        /// 部分文件夹不加入命名空间。
        /// </summary>
        private static readonly Regex IgnoreIdentifierRegex = new Regex(@"^([Bb]in|[Oo]bj|[Dd]ebug|[Rr]elease|[Xx]86|[Xx]64|net[\.\w]*\d+(-\w+)?)$");

        /// <summary>
        /// 一部分符合要求的命名会被忽略形成命名空间。
        /// </summary>
        private static readonly Regex IgnoreIdentifierNamingRegex = new Regex(@"^\..+$|^_.+$|.+_$");

        public static (string @namespace, string @class) MakeNamespaceAndClassName(DirectoryInfo projectDirectory, FileInfo yamlFile, string rootNamespace)
        {
            var yamlDirectory = yamlFile.Directory;
            var relative = PathHelper.MakeRelativePath(projectDirectory.FullName, yamlDirectory.FullName);
            var pathParts = relative.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            var namespaceParts = new List<string>();
            foreach (var part in pathParts)
            {
                var identifier = ConvertAnyStringToIdentifier(part);
                if (identifier.Length > 0)
                {
                    namespaceParts.Add(identifier);
                }
            }

            var namespacePart = string.Join(".", namespaceParts);
            var yamlFileName = Path.GetFileNameWithoutExtension(yamlFile.FullName);
            var classPart = ConvertAnyStringToIdentifier(yamlFileName);

            var @namespace = string.IsNullOrWhiteSpace(namespacePart)
                ? rootNamespace
                : $"{rootNamespace}.{namespacePart}";
            var @class = string.IsNullOrWhiteSpace(classPart)
                ? "UnamedYamlObject"
                : classPart;

            return (@namespace, @class);
        }

        private static string ConvertAnyStringToIdentifier(string text)
        {
            var containsLetter = text.Any(x => char.IsLetter(x));
            if (!containsLetter)
            {
                return "";
            }

            if (IgnoreIdentifierRegex.Match(text).Success
                || IgnoreIdentifierNamingRegex.Match(text).Success)
            {
                // 所有符合文档中规定的文件夹命名规则的名称都不加入到命名空间。
                return "";
            }

            var partBuilder = new StringBuilder();
            foreach (var c in text)
            {
                if (partBuilder.Length == 0)
                {
                    if (char.IsLetter(c))
                    {
                        partBuilder.Append(c);
                    }
                }
                else
                {
                    var last = partBuilder[partBuilder.Length - 1];
                    if (char.IsLetterOrDigit(c))
                    {
                        partBuilder.Append(c);
                    }
                    else if (char.IsLetterOrDigit(last))
                    {
                        partBuilder.Append('_');
                    }
                }
            }

            return partBuilder.ToString();
        }
    }
}
