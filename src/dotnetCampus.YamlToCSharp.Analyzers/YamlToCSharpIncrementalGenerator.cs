using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using dotnetCampus.YamlToCSharp.Utils;

using Microsoft.CodeAnalysis;

namespace dotnetCampus.YamlToCSharp.Analyzers;

[Generator(LanguageNames.CSharp)]
public class YamlToCSharpIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        Debugger.Launch();
        Debugger.Break();

        var incrementalValuesProvider = context.AdditionalTextsProvider.Where(t =>
        {
            var extension = Path.GetExtension(t.Path);
            return string.Equals(extension, ".yml", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".yaml", StringComparison.OrdinalIgnoreCase);
        });

        context.RegisterSourceOutput(incrementalValuesProvider, (sourceProductionContext, ymlText) =>
        {
           

            var projectDirectory = FileProjectDirectory(ymlText.Path);
            var (classNamespace, className) = IdentifierHelper.MakeNamespaceAndClassName(projectDirectory, new FileInfo(ymlText.Path), "dotnetCampus.Localizations");

            var sourceFileName = className;

            var sourceText = ymlText.GetText();
            if (sourceText != null)
            {
                var yamlText = sourceText.ToString();
                var yamlFileToCSharpFile = new YamlFileToCSharpFile();
                var code = yamlFileToCSharpFile.YamlToCsharpCode(yamlText, className, classNamespace);
                sourceProductionContext.AddSource(sourceFileName, code);
            }
            else
            {
                // 如果原先的被删除了，那就是拿到了空白的内容，返回空即可
                sourceProductionContext.AddSource(sourceFileName, string.Empty);
            }
        });
    }

    private DirectoryInfo FileProjectDirectory(string fileName)
    {
        // 第一层的文件夹一定存在
        var currentDirectory = Path.GetDirectoryName(fileName)!;

        var directory = currentDirectory;
        while (directory is not null)
        {
            if (Directory.GetFiles(directory, "*.csproj", SearchOption.TopDirectoryOnly).Any())
            {
                // 如果能找到 csproj 文件，那就是项目所在文件夹了
                return new DirectoryInfo(directory);
            }

            directory = Path.GetDirectoryName(directory);
        }

        return new DirectoryInfo(currentDirectory);
    }

    private string FileNameToClassName(string fileName)
    {
        return fileName.Replace("-", "_").Replace(".", "_").Replace(" ", "_");
    }
}