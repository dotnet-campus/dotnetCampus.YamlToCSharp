using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using dotnetCampus.Cli;
using dotnetCampus.YamlToCSharp.Core;
using dotnetCampus.YamlToCSharp.Utils;

namespace dotnetCampus.YamlToCSharp.Cli
{
    internal class CompileTask
    {
        /// <summary>
        /// 要转换 YAML 的目标项目的路径。
        /// </summary>
        [Option(nameof(ProjectDirectory))]
        public string? ProjectDirectory { get; set; }

        /// <summary>
        /// 转换代码时应该使用的工作路径。
        /// </summary>
        [Option(nameof(WorkingDirectory))]
        public string? WorkingDirectory { get; set; }

        /// <summary>
        /// 项目的根命名空间。
        /// </summary>
        [Option(nameof(RootNamespace))]
        public string? RootNamespace { get; set; }

        /// <summary>
        /// 要转换的 YAML 文件的路径，使用分号分隔。建议使用 MSBuild 来生成这样的集合，形如 @(xxx)。
        /// </summary>
        [Option(nameof(YamlSourceFiles))]
        public string? YamlSourceFiles { get; set; }

        internal void Run()
        {
            var projectDirectoryString = ProjectDirectory;
            if (projectDirectoryString is null)
            {
                throw new ArgumentException("必须指定项目路径。", nameof(ProjectDirectory));
            }

            var workingDirectoryString = WorkingDirectory;
            if (workingDirectoryString is null)
            {
                throw new ArgumentException("必须指定项目工作路径，IntermediateOutputPath。", nameof(WorkingDirectory));
            }

            var rootNamespace = RootNamespace;
            if (rootNamespace is null || string.IsNullOrWhiteSpace(rootNamespace))
            {
                throw new ArgumentException("必须指定项目的根命名空间，RootNamespace。", nameof(RootNamespace));
            }

            var yamlFiles = YamlSourceFiles?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (yamlFiles is null || yamlFiles.Length == 0)
            {
                YC.Logger.Warning("没有指定任何需要转换为 C# 代码的 YAML 文件。是否忘记在项目中设置 <YamlToCSharpFile Include=\"Yaml\\**\\*.yml\" />？");
                return;
            }

            var projectDirectory = new DirectoryInfo(projectDirectoryString);
            var workingDirectory = Path.IsPathRooted(workingDirectoryString)
                ? new DirectoryInfo(workingDirectoryString)
                : new DirectoryInfo(Path.Combine(projectDirectory.FullName, workingDirectoryString));

            var convertedFiles = RunCore(projectDirectory, workingDirectory, rootNamespace,
                yamlFiles.Select(x => new FileInfo(Path.Combine(projectDirectory.FullName, x))));

            File.WriteAllText(
                Path.Combine(workingDirectory.FullName, "OutputSourceFiles.txt"),
                string.Join(";", convertedFiles.Select(x => x.FullName)));
        }

        private IReadOnlyList<FileInfo> RunCore(
            DirectoryInfo projectDirectory, DirectoryInfo workingDirectory,
            string rootNamespace, IEnumerable<FileInfo> yamlFiles)
        {
            const string methodName = "AsDictionary";
            const string interfaceName = "IYamlCSharpDictionary";
            var convertedFiles = new List<FileInfo>();

            // 生成接口。
            var interfaceCreator = new ShapeInterfaceCreator(
                rootNamespace, interfaceName,
                $"System.Collections.Generic.Dictionary<string, string> {methodName}()");
            var interfaceFile = new FileInfo(Path.Combine(workingDirectory.FullName, $"{interfaceName}.cs"));
            var @interface = interfaceCreator.Create();
            File.WriteAllText(interfaceFile.FullName, @interface);
            convertedFiles.Add(interfaceFile);

            // 生成实现类。
            var yamlFileToCSharpFile = new YamlFileToCSharpFile();
            foreach (var yamlFile in yamlFiles)
            {
                var (@namespace, @class) = IdentifierHelper.MakeNamespaceAndClassName(projectDirectory, yamlFile, rootNamespace);
                var csharpFile = new FileInfo(Path.Combine(workingDirectory.FullName, $"{@namespace}.{@class}.cs"));

                yamlFileToCSharpFile.ParseToCSharpFile(yamlFile, csharpFile,
                    @namespace,
                    $"{RootNamespace}.{interfaceName}",
                    className: @class,
                    methodName: "methodName");

                convertedFiles.Add(csharpFile);
            }

            return convertedFiles;
        }
    }
}
