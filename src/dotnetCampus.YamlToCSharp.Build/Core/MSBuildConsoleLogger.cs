using System;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// 报告编译警告。
        /// </summary>
        /// <param name="message">编译警告消息。</param>
        /// <param name="filePath">自动传入或手工指定，要报告错误的文件，可用于 Visual Studio 自动定位到目标文件方便调试错误。</param>
        /// <param name="line">自动传入或手工指定，要报告错误的文件的行号，可用于 Visual Studio 自动定位到目标文件的对应位置方便调试错误。</param>
        public static void BuildWarning(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 0)
        {
            Console.WriteLine($"{filePath}({line},0,{line},1) warning: {message}");
        }

        /// <summary>
        /// 报告编译错误。
        /// </summary>
        /// <param name="message">编译错误消息。</param>
        /// <param name="filePath">自动传入或手工指定，要报告错误的文件，可用于 Visual Studio 自动定位到目标文件方便调试错误。</param>
        /// <param name="line">自动传入或手工指定，要报告错误的文件的行号，可用于 Visual Studio 自动定位到目标文件的对应位置方便调试错误。</param>
        public static void BuildError(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 0)
        {
            Console.WriteLine($"{filePath}({line},0,{line},1) error: {message}");
        }

        /// <summary>
        /// 立即抛出异常。除报告编译错误外，立刻停止后续的编译过程。
        /// </summary>
        /// <param name="message">编译错误消息。</param>
        /// <param name="filePath">自动传入或手工指定，要报告错误的文件，可用于 Visual Studio 自动定位到目标文件方便调试错误。</param>
        /// <param name="line">自动传入或手工指定，要报告错误的文件的行号，可用于 Visual Studio 自动定位到目标文件的对应位置方便调试错误。</param>
        public static void BuildThrow(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 0)
        {
            throw new MSBuildException(message, filePath, line);
        }
    }
}
