# dotnetCampus.YamlToCSharp

将 YAML 文件转 C# 代码（字典）。

## NuGet

本仓库提供三个 NuGet 包：

| 包                               | NuGet                                                        | 作用         |
| -------------------------------- | ------------------------------------------------------------ | ------------ |
| dotnetCampus.YamlToCSharp        | [![dotnetCampus.YamlToCSharp](https://img.shields.io/nuget/v/dotnetCampus.YamlToCSharp)](https://www.nuget.org/packages/dotnetCampus.YamlToCSharp/) | 依赖库[^1]   |
| dotnetCampus.YamlToCSharp.Source | [![dotnetCampus.YamlToCSharp.Source](https://img.shields.io/nuget/v/dotnetCampus.YamlToCSharp.Source)](https://www.nuget.org/packages/dotnetCampus.YamlToCSharp.Source/) | 源代码包[^2] |
| dotnetCampus.YamlToCSharp.Build  | [![dotnetCampus.YamlToCSharp.Build](https://img.shields.io/nuget/v/dotnetCampus.YamlToCSharp.Build)](https://www.nuget.org/packages/dotnetCampus.YamlToCSharp.Build/) | 编译依赖[^3] |

[^1]: 提供将 YAML 文件转换成 C# 代码的能力。
[^2]: 源代码包，不会额外为你的项目引入依赖。提供将 YAML 文件转换成 C# 代码的能力。
[^3]: 在你的项目编译时，将 YAML 文件转换成 C# 代码。

## 使用

### 库

安装 [dotnetCampus.YamlToCSharp](https://www.nuget.org/packages/dotnetCampus.YamlToCSharp/) 到你的项目中，即可使用 YAML 转 C# 相关的辅助类。

典型的用法是生成语言项，你可以阅读下面的博客：

- [dotnet 通过 dotnetCampus.YamlToCSharp 将 YAML 多语言文件构建为代码](https://blog.lindexi.com/post/dotnet-%E9%80%9A%E8%BF%87-dotnetCampus.YamlToCsharp-%E5%B0%86-YAML-%E5%A4%9A%E8%AF%AD%E8%A8%80%E6%96%87%E4%BB%B6%E6%9E%84%E5%BB%BA%E4%B8%BA%E4%BB%A3%E7%A0%81.html)

### 自动编译

安装 [dotnetCampus.YamlToCSharp.Build](https://www.nuget.org/packages/dotnetCampus.YamlToCSharp.Build/) 到你的项目中，即可使用已经自动编译好的 YAML 转 C# 的类：

假设你的项目中存在 Localizations 文件夹，其中的文件结构如下：

```
- /Localizations
    - en-US
        - Extension.yaml
        - Main.yml
    - zh-CN
        - Extension.yaml
        - Main.yml
    - zh-TW
        - Extension.yaml
        - Main.yml
```

那么你可以在代码中编写如下 C# 代码来使用这些 YAML 文件中的内容，视为字典：

```csharp
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
```

默认情况下，会将你项目中所有的 *.yml 文件和 *.yaml 文件加入编译，就像写了下面代码一样：

```xml
<ItemGroup>
    <YamlToCSharpCompile Include="**\*.yml;**\*.yaml" Exclude="obj\**\*.yml;obj\**\*.yaml;bin\**\*.yml;bin\**\*.yaml" />
</ItemGroup>
```

默认情况下，通过 YAML 生成的 C# 文件的命名空间就是在 YAML 文件夹中创建一个 C# 代码时的命名空间一样；而生成的类名就是文件名进行标识符处理后的名字。但，你也可以改变这一行为。

有两种方法来更改生成类的命名空间：

1. 使用特殊名称的文件夹来限定命名空间；
2. 在 `YamlToCSharpCompile` 中设置额外的属性（暂未实现）。

特殊命名的文件夹有：`bin`、`obj`、`debug`、`release`、`x86`、`x64`、`net48`（和其他 .NET Framework 框架版本）、`netcoreapp3.1`（和其他 .NET Core 框架版本）；以及所有以点（`.`）开头的文件夹（如 `.vs`），所有以下画线（`_`）开头或结尾的文件夹（例如 `_test`、`test_` ）。

<!--
以下供正则测试

正例：

bin
Bin
obj
Obj
debug
Debug
release
Release
x86
X86
x64
X64
net45
netstandard2.0
netcoreapp3.1
net5.0
.git
.vs
_test
test_

反例：

xbin
bing
net2.
test
-->

设置额外的属性如下：

```xml
<ItemGroup>
    <YamlToCSharpCompile Update="Loc\a.yml" Namespace="dotnetCampus.Demo" ClassName="Foo" />
</ItemGroup>
```

`Namespace` 设置生成的类型的命名空间，`ClassName` 设置生成的类型的类名。不设置则继续保持默认值。假设项目的默认命名空间为 `dotnetCampus.YamlToCSharp`，那么在以上示例代码编写前，生成的类是 `dotnetCampus.YamlToCSharp.Loc.a`，在编写后生成的类是 `dotnetCampus.Demo.Foo`。

### 扩展自动编译

安装 [dotnetCampus.YamlToCSharp.Build](https://www.nuget.org/packages/dotnetCampus.YamlToCSharp.Build/) 到你的项目中之后，你可以扩展 YAML 到 C# 的转换过程：

```xml
<!-- 在 YAML 向 C# 编译之前，做些什么。这个时机可以改变要编译的 YAML 文件 -->
<Target Name="CustomTarget"
        BeforeTargets="YamlToCSharpCoreCompile">
    <ItemGroup>

        <!-- 将收集到的要编译的 YAML 文件存到 CollectedYamlToCSharpCompile 集合中。 -->
        <CollectedYamlToCSharpCompile Include="@(YamlToCSharpCompile)" />

        <!-- 清空默认引入的全部要编译的 YAML 文件。 -->
        <YamlToCSharpCompile Remove="**\*.yml;**\*.yaml" />

    </ItemGroup>
</Target>
```

```xml
<!-- 在 YAML 向 C# 编译之后，做些什么。这个时机可以拿到所有已编译好的 C# 文件。 -->
<Target Name="CustomTarget"
        AfterTargets="YamlToCSharpCoreCompile">
    <ItemGroup>

        <!-- 已从 YAML 编译好的 C# 文件。 -->
        <CollectedYamlToCSharpCompile Include="@(YamlCompiledCSharp)" />

    </ItemGroup>
</Target>
```

## 贡献

### 报告问题

当项目出现编译错误，并提示你报告问题时，建议在这里报告问题

### 调试本项目

1. 编译 dotnetCampus.YamlToCSharp.Sample.Wpf 项目，可以模拟 dotnetCampus.YamlToCSharp.Build 包在目标项目中编译时的效果。
