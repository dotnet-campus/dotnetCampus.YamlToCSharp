using System;
using System.Runtime.Serialization;

namespace dotnetCampus.YamlToCSharp.Build.Core
{
    [Serializable]
    public class MSBuildException : Exception
    {
        public readonly string _message;

        public MSBuildException()
        {
            _message = "An unknown error occurred during the compilation.";
        }

        public MSBuildException(string message)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public MSBuildException(string message, Exception innerException) : base(message, innerException)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }

        protected MSBuildException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            _message ??= "An unknown error occurred during the compilation.";
        }

        public string MSBuildMessage => $"{_message}";
    }
}
