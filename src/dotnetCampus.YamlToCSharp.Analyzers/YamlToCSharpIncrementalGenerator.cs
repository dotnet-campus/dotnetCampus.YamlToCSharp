using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using dotnetCampus.YamlToCSharp.Utils;

using Microsoft.CodeAnalysis;

namespace dotnetCampus.YamlToCSharp.Analyzers;

[Generator(LanguageNames.CSharp)]
public class YamlToCSharpIncrementalGenerator : IIncrementalGenerator
{
    

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //Debugger.Launch();
        //Debugger.Break();

        var yamlFileProvider = context.AdditionalTextsProvider.Where(t =>
        {
            var extension = Path.GetExtension(t.Path);
            return string.Equals(extension, ".yml", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".yaml", StringComparison.OrdinalIgnoreCase);
        });

        IncrementalValuesProvider<(string sourceFileName, string code)> csharpCodeProvider = yamlFileProvider.Select((ymlText, token) =>
        {
            var projectDirectory = FileProjectDirectory(ymlText.Path);
            var (classNamespace, className) = IdentifierHelper.MakeNamespaceAndClassName(projectDirectory,
                new FileInfo(ymlText.Path), "dotnetCampus.Localizations");

            var sourceFileName = classNamespace + "." + className + ".yml" + ".cs";

            var sourceText = ymlText.GetText(token);
            if (sourceText != null)
            {
                TryLoadYamlDotNet();

                var yamlText = sourceText.ToString();
                var yamlFileToCSharpFile = new YamlFileToCSharpFile();
                var code = yamlFileToCSharpFile.YamlToCsharpCode(yamlText, className, classNamespace,
                    needAddPartial: false);
                return (sourceFileName, code);
            }

            return (sourceFileName, string.Empty);
        });

        context.RegisterSourceOutput(csharpCodeProvider, (sourceProductionContext, provider) =>
        {
            var (sourceFileName, code) = provider;

            Debugger.Launch();

            if (!sourceProductionContext.CancellationToken.IsCancellationRequested)
            {
                sourceProductionContext.AddSource(sourceFileName, code);
            }
            //var projectDirectory = FileProjectDirectory(ymlText.Path);
            //var (classNamespace, className) = IdentifierHelper.MakeNamespaceAndClassName(projectDirectory, new FileInfo(ymlText.Path), "dotnetCampus.Localizations");

            //var sourceFileName = classNamespace + "." + className + ".yml" + ".cs";

            //var sourceText = ymlText.GetText();
            //if (sourceText != null)
            //{
            //    TryLoadYamlDotNet();

            //    var yamlText = sourceText.ToString();
            //    var yamlFileToCSharpFile = new YamlFileToCSharpFile();
            //    var code = yamlFileToCSharpFile.YamlToCsharpCode(yamlText, className, classNamespace, needAddPartial: false);

            //}
            //else
            //{
            //    // 如果原先的被删除了，那就是拿到了空白的内容，返回空即可
            //    sourceProductionContext.AddSource(sourceFileName, string.Empty);
            //}
        });
    }

    private static void TryLoadYamlDotNet()
    {
        // 尝试加上 YamlDotNet.dll 文件，由于源代码生成没有拷贝依赖，需要手动加载
        // [Analyzer not working using Nuget · Issue #56076 · dotnet/roslyn](https://github.com/dotnet/roslyn/issues/56076)
        // [I can't use source generator with package reference · Issue #56024 · dotnet/roslyn](https://github.com/dotnet/roslyn/issues/56024)

        // YamlDotNet, Version=11.0.0.0, Culture=neutral, PublicKeyToken=ec19458f3c15af5e
        var yamlDotNetAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.FullName.Contains("YamlDotNet,"));
        if (yamlDotNetAssembly != null)
        {
            // 加载成功
            return;
        }

        var assembly = Assembly.GetExecutingAssembly();
        var location = assembly.Location;
        var directory = Path.GetDirectoryName(location)!;
        var yamlDotNetStream = assembly.GetManifestResourceStream("dotnetCampus.YamlToCSharp.Analyzers.YamlDotNet.dll")!;
        var yamlDotNetFile = Path.Combine(directory, "YamlDotNet.dll");
        using (var fileStream = File.OpenWrite(yamlDotNetFile))
        {
            yamlDotNetStream.CopyTo(fileStream);
        }

        Assembly.LoadFrom(yamlDotNetFile);
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