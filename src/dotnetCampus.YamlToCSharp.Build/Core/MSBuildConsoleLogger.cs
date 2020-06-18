using System;

using Walterlv.Logging;
using Walterlv.Logging.Core;

namespace dotnetCampus.YamlToCSharp.Core
{
    internal class MSBuildConsoleLogger : OutputLogger
    {
        protected override void OnInitialized()
        {
        }

        protected override void OnLogReceived(in LogContext context)
        {
            switch (context.CurrentLevel)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Fatal:
                case LogLevel.Error:
                    Console.WriteLine($"error: {context.Text}{(context.ExtraInfo is null ? "" : $" 内部错误：{context.ExtraInfo}")}");
                    break;
                case LogLevel.Warning:
                    Console.WriteLine($"warning: {context.Text} {context.ExtraInfo}");
                    break;
                case LogLevel.Message:
                case LogLevel.Detail:
                    Console.WriteLine(context.Text + context.ExtraInfo);
                    break;
            }
        }
    }
}
