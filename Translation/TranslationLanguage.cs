using System.ComponentModel;

namespace PluginCore;

public enum TranslationSourceLanguage
{
    [Description("lang.kitopia.translation.auto_detect")]
    Auto,
    [Description("lang.kitopia.translation.simplified_chinese")]
    SimplifiedChinese,
    [Description("lang.kitopia.translation.traditional_chinese")]
    TraditionalChinese,
    [Description("lang.kitopia.translation.english")]
    English,
    [Description("lang.kitopia.translation.japanese")]
    Japanese
}

public enum TranslationTargetLanguage
{
    [Description("lang.kitopia.translation.simplified_chinese")]
    SimplifiedChinese,
    [Description("lang.kitopia.translation.traditional_chinese")]
    TraditionalChinese,
    [Description("lang.kitopia.translation.english")]
    English,
    [Description("lang.kitopia.translation.japanese")]
    Japanese
}
