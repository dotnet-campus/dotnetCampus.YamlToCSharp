using Walterlv.Logging;

namespace dotnetCampus.YamlToCSharp.Core
{
    internal static class YC
    {
        public static ILogger Logger { get; } = new MSBuildConsoleLogger();
    }
}
