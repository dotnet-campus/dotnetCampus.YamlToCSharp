using System;

using dotnetCampus.Cli;

namespace dotnetCampus.YamlToCSharp
{
    internal class Options
    {
        /// <summary>
        /// 要转换的 YAML 文件夹所在的路径。
        /// </summary>
        [Value(0), Option('i', "YamlSourceDirectory")]
        public string? YamlSourceDirectory { get; set; }

        internal void Run()
        {

        }
    }
}
