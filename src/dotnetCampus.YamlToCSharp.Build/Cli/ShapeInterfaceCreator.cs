using System;

namespace dotnetCampus.YamlToCSharp.Cli
{
    internal class ShapeInterfaceCreator
    {
        private readonly string _namespace;
        private readonly string _interfaceName;
        private readonly string _methodSignature;

        public ShapeInterfaceCreator(string @namespace, string interfaceName, string methodSignature)
        {
            _namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
            _interfaceName = interfaceName ?? throw new ArgumentNullException(nameof(interfaceName));
            _methodSignature = methodSignature ?? throw new ArgumentNullException(nameof(methodSignature));
        }

        public string Create()
        {
            return $@"
namespace {_namespace}
{{
    internal interface {_interfaceName}
    {{
        {_methodSignature};
    }}
}}
";
        }
    }
}
