using System.Collections.Generic;

namespace dotnetCampus.YamlToCSharp.SourceGeneration.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        static Dictionary<string, Dictionary<string, string>[]> GetLangs()
        {
            return new Dictionary<string, Dictionary<string, string>[]>()
            {
                {
                    "zh-CN", new[]
                    {
                        new Localizations.zh_CN.Main().GetLang(),
                        new Localizations.zh_CN.Extension().GetLang(),
                    }
                },
                {
                    "zh-TW", new[]
                    {
                        new Localizations.zh_TW.Main().GetLang(),
                        new Localizations.zh_TW.Extension().GetLang(),
                    }
                },
                {
                    "en-US", new[]
                    {
                        new Localizations.en_US.Main().GetLang(),
                        new Localizations.en_US.Extension().GetLang(),
                    }
                },
            };
        }
    }
}