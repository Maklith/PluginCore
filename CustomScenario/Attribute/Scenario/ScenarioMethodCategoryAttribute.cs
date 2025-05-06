using System;

namespace PluginCore.Attribute.Scenario;

/// <summary>
/// 设置类在<see cref="Scenario"/>中的分类
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ScenarioMethodCategoryAttribute : System.Attribute
{
    public bool IsMixinOrTopCategory { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// name和isMixinOrTopCategory参数具体如下
    /// <code>
    /// |
    /// |- Kitopia
    ///       |
    ///       |-分类1 (name="Kitopia/分类1", isMixinOrTopCategory=true)
    /// |- 插件名称(本插件)
    ///       |-分类1 (name="分类1", isMixinOrTopCategory=false)
    ///       |-分类2 (name="分类2", isMixinOrTopCategory=false)
    /// |- 其他插件名称
    ///      |- 分类1 (name="其他插件名称/分类1", isMixinOrTopCategory=true) 
    /// </code>
    /// </summary>
    /// <param name="name">分类名称</param>
    /// <param name="isMixinOrTopCategory">是否为顶级分类，如果为false只能添加到该插件分类下的同名分类</param>
    /// 
    public ScenarioMethodCategoryAttribute(string name, bool isMixinOrTopCategory = false)
    {
        Name = name;
        IsMixinOrTopCategory = isMixinOrTopCategory;
    }
}