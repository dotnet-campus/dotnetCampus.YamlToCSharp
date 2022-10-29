using System;
using Microsoft.CodeAnalysis;

namespace dotnetCampus.YamlToCSharp.Analyzers;

[Generator(LanguageNames.CSharp)]
public class YamlToCSharpIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        
    }
}