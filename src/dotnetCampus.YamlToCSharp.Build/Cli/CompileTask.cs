using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

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

        /// <summary>
        /// 转换后的源代码文件目录存放在这个文件内。
        /// </summary>
        [Option(nameof(OutputIndexFile))]
        public string? OutputIndexFile { get; set; }

        [Option("Debug")]
        public bool DebugMode { get; set; } = false;

        internal void Run()
        {
            var projectDirectoryString = ProjectDirectory?.Trim();
            if (projectDirectoryString is null)
            {
                throw new ArgumentException("必须指定项目路径。", nameof(ProjectDirectory));
            }
            var projectDirectory = new DirectoryInfo(projectDirectoryString);

            var workingDirectoryString = WorkingDirectory?.Trim();
            if (workingDirectoryString is null)
            {
                throw new ArgumentException("必须指定项目工作路径，IntermediateOutputPath。", nameof(WorkingDirectory));
            }
            var workingDirectory = Path.IsPathRooted(workingDirectoryString)
                ? new DirectoryInfo(workingDirectoryString)
                : new DirectoryInfo(Path.Combine(projectDirectory.FullName, workingDirectoryString));

            var rootNamespace = RootNamespace?.Trim();
            if (rootNamespace is null || string.IsNullOrWhiteSpace(rootNamespace))
            {
                throw new ArgumentException("必须指定项目的根命名空间，RootNamespace。", nameof(RootNamespace));
            }

            if (DebugMode is true)
            {
                Debugger.Launch();
                Debugger.Break();
            }

            var yamlFiles = YamlSourceFiles?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim())
                .Select(x => new FileInfo(Path.Combine(projectDirectory.FullName, x))).ToArray();
            if (yamlFiles is null || yamlFiles.Length == 0)
            {
                YC.Logger.Warning("没有指定任何需要转换为 C# 代码的 YAML 文件。是否忘记在项目中设置 <YamlToCSharpFile Include=\"Yaml\\**\\*.yml\" />？");
                yamlFiles ??= new FileInfo[0];
            }

            var outputIndexFileString = OutputIndexFile?.Trim();
            if (outputIndexFileString is null)
            {
                throw new ArgumentException("必须指定输出源代码的目录文件，OutputIndexFile。", nameof(OutputIndexFile));
            }
            var outputIndexFile = Path.IsPathRooted(outputIndexFileString)
                ? new FileInfo(outputIndexFileString)
                : new FileInfo(Path.Combine(projectDirectory.FullName, outputIndexFileString));

            var convertedFiles = RunCore(projectDirectory, workingDirectory, rootNamespace, yamlFiles);

            File.WriteAllText(
                outputIndexFile.FullName,
                string.Join(Environment.NewLine, convertedFiles.Select(x => x.FullName)));
        }

        private IReadOnlyList<FileInfo> RunCore(
            DirectoryInfo projectDirectory, DirectoryInfo workingDirectory,
            string rootNamespace, IEnumerable<FileInfo> yamlFiles)
        {
            const string methodName = "AsDictionary";
            const string interfaceName = "IYamlCSharpDictionary";
            var convertedFiles = new List<FileInfo>();
            workingDirectory.Create();

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
                    @namespace, $"{RootNamespace}.{interfaceName}", @class, methodName,
                    Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);

                convertedFiles.Add(csharpFile);
            }

            return convertedFiles;
        }
    }
}
