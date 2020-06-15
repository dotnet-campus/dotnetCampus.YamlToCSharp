using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace dotnetCampus.YamlToCSharp.Utils
{
    internal static class IdentifierHelper
    {
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
