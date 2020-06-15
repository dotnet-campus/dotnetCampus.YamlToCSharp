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

            var languages = new List<IYamlCSharpDictionary>
            {
                new dotnetCampus.YamlToCSharp.Localizations.en_US.Main(),
                new dotnetCampus.YamlToCSharp.Localizations.en_US.Extension(),
                new dotnetCampus.YamlToCSharp.Localizations.zh_CN.Main(),
                new dotnetCampus.YamlToCSharp.Localizations.zh_CN.Extension(),
                new dotnetCampus.YamlToCSharp.Localizations.zh_TW.Main(),
                new dotnetCampus.YamlToCSharp.Localizations.zh_TW.Extension(),
            };
            var dict = languages.SelectMany(x => x.AsDictionary());
        }
    }
}
