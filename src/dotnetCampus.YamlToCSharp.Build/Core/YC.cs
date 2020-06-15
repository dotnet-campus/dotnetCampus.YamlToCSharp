using Walterlv.Logging;

namespace dotnetCampus.YamlToCSharp.Build.Core
{
    internal static class YC
    {
        public static ILogger Logger { get; } = new MSBuildConsoleLogger();
    }
}
