using System;
using System.Runtime.Serialization;
using System.Text;

namespace dotnetCampus.YamlToCSharp.Core
{
    [Serializable]
    public class MSBuildException : Exception
    {
        private readonly string _message;
        private readonly string? _filePath;
        private readonly int? _lineStart;
        private readonly int? _columnStart;
        private readonly int? _lineEnd;
        private readonly int? _columnEnd;

        public MSBuildException()
        {
            _message = "An unknown error occurred during the compilation.";
        }

        public MSBuildException(string message, string? filePath = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
            _filePath = filePath;
            _lineStart = lineStart;
            _columnStart = columnStart;
            _lineEnd = lineEnd;
            _columnEnd = columnEnd;
        }

        public MSBuildException(string message, Exception innerException, string? filePath = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null)
            : base(message, innerException)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
            _filePath = filePath;
            _lineStart = lineStart;
            _columnStart = columnStart;
            _lineEnd = lineEnd;
            _columnEnd = columnEnd;
        }

        protected MSBuildException(SerializationInfo serializationInfo, StreamingContext streamingContext) : this()
        {
        }

        public string BuildMSBuildMessage()
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(_filePath))
            {
                builder.Append(_filePath);
            }
            if (_lineStart != null || _lineEnd != null)
            {
                var lineStart = _lineStart ?? _lineEnd;
                var lineEnd = _lineEnd ?? lineStart;
                var columnStart = _columnStart ?? 0;
                var columnEnd = _columnEnd ?? 1;
                builder.Append('(').Append(_lineStart).Append(',').Append(columnStart).Append(',').Append(lineEnd).Append(',').Append(columnEnd).Append(')');
            }
            if (builder.Length > 0)
            {
                builder.Append(' ');
            }
            builder.Append("error: ");
            builder.Append(_message);
            return builder.ToString();
        }

        public void ReportBuildError()
        {
            var message = BuildMSBuildMessage();
            Console.WriteLine(message);
        }
    }
}
