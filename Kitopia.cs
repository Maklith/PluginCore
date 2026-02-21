using System;
using System.Collections.Generic;
using PluginCore.CustomScenario;
using PluginCore.Onnx;
using Serilog.Core;

namespace PluginCore;

public class Kitopia
{
    public static IServiceProvider ServiceProvider;
    public static ISearchItemTool ISearchItemTool;
    public static IClipboardService IClipboardService;
    public static IToastService IToastService;
    public static Dictionary<string, string> _i18n;
    public static Dictionary<Type, Func<object, string>> ToolTipConverters;
    public static Dictionary<Type, ICustomScenarioValueSerializer> JsonConverters;
    public static IInferenceSessionManager InferenceSessionManager;
    public static Logger Logger;
}