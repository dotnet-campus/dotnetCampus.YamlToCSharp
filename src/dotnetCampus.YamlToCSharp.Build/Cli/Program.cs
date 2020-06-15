using System;

using dotnetCampus.Cli;
using dotnetCampus.YamlToCSharp.Core;

namespace dotnetCampus.YamlToCSharp.Cli
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var options = CommandLine.Parse(args)
                    .AddHandler<CompileTask>(o => o.Run())
                    .Run();
            }
            catch (ArgumentException ex)
            {
                YC.Logger.Error("命令行参数错误，这是 YamlToCSharp 库的内部问题，请报告 Issue：https://github.com/dotnet-campus/dotnetCampus.YamlToCSharp/issues/new", ex);
            }
            catch (MSBuildException ex)
            {
                YC.Logger.Error(ex.MSBuildMessage);
            }
            catch (Exception ex)
            {
                YC.Logger.Error("发生了未知错误，这是 YamlToCSharp 库的内部问题，请报告 Issue：https://github.com/dotnet-campus/dotnetCampus.YamlToCSharp/issues/new", ex);
            }
        }
    }
}
