using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace dotnetCampus.YamlToCSharp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var languages = new Dictionary<string, IYamlCSharpDictionary[]>
            {
                {
                    "en-US", new IYamlCSharpDictionary[]
                    {
                        new dotnetCampus.YamlToCSharp.Localizations.en_US.Main(),
                        new dotnetCampus.YamlToCSharp.Localizations.en_US.Extension(),
                    }
                },
                {
                    "zh-CN", new IYamlCSharpDictionary[]
                    {
                        new dotnetCampus.YamlToCSharp.Localizations.zh_CN.Main(),
                        new dotnetCampus.YamlToCSharp.Localizations.zh_CN.Extension(),
                    }
                },
                {
                    "zh-TW", new IYamlCSharpDictionary[]
                    {
                        new dotnetCampus.YamlToCSharp.Localizations.zh_TW.Main(),
                        new dotnetCampus.YamlToCSharp.Localizations.zh_TW.Extension(),
                    }
                },
            };
            var dict = languages["zh-CN"].SelectMany(x => x.AsDictionary());
        }
    }
}
