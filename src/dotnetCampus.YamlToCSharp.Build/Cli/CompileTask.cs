using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using dotnetCampus.Cli;
using dotnetCampus.YamlToCSharp.Core;

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
        /// 要转换的 YAML 文件的路径，使用分号分隔。建议使用 MSBuild 来生成这样的集合，形如 @(xxx)。
        /// </summary>
        [Option(nameof(YamlSourceFiles))]
        public string? YamlSourceFiles { get; set; }

        /// <summary>
        /// 项目的根命名空间。
        /// </summary>
        [Option(nameof(RootNamespace))]
        public string? RootNamespace { get; set; }

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

            RunCore(workingDirectory, RootNamespace, yamlFiles.Select(x => Path.Combine(projectDirectory.FullName, x)));
        }

        private void RunCore(DirectoryInfo workingDirectory, string rootNamespace, IEnumerable<string> yamlFiles)
        {

        }
    }
}
