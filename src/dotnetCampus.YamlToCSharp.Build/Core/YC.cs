using Walterlv.Logging;

namespace dotnetCampus.YamlToCSharp.Core
{
    internal static class YC
    {
        public static MSBuildConsoleLogger Logger { get; } = new MSBuildConsoleLogger();
    }
}
