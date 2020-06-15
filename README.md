# dotnetCampus.YamlToCSharp

将 YAML 文件转 C# 代码（字典）。

本仓库提供三个 NuGet 包：

| 包                               | NuGet                                                        | 作用         |
| -------------------------------- | ------------------------------------------------------------ | ------------ |
| dotnetCampus.YamlToCSharp        | [![dotnetCampus.YamlToCSharp](https://img.shields.io/nuget/v/dotnetCampus.YamlToCSharp)](https://www.nuget.org/packages/dotnetCampus.YamlToCSharp/) | 依赖库[^1]   |
| dotnetCampus.YamlToCSharp.Source | [![dotnetCampus.YamlToCSharp.Source](https://img.shields.io/nuget/v/dotnetCampus.YamlToCSharp.Source)](https://www.nuget.org/packages/dotnetCampus.YamlToCSharp.Source/) | 源代码包[^2] |
| dotnetCampus.YamlToCSharp.Build  | [![dotnetCampus.YamlToCSharp.Build](https://img.shields.io/nuget/v/dotnetCampus.YamlToCSharp.Build)](https://www.nuget.org/packages/dotnetCampus.YamlToCSharp.Build/) | 编译依赖[^3] |

[^1]: 提供将 YAML 文件转换成 C# 代码的能力。
[^2]: 源代码包，不会额外为你的项目引入依赖。提供将 YAML 文件转换成 C# 代码的能力。
[^3]: 在你的项目编译时，将 YAML 文件转换成 C# 代码。