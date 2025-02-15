using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Core.SDKs.CustomScenario;
using Core.SDKs.Services;
using PluginCore.Onnx;

namespace PluginCore;

public class Kitopia
{
    public static ISearchItemTool ISearchItemTool;
    public static IClipboardService IClipboardService;
    public static IToastService IToastService;
    public static Dictionary<string, string> _i18n;
    public static Dictionary<Type, Func<object, string>> ToolTipConverters;
    public static Dictionary<Type, ICustomScenarioValueSerializer> JsonConverters;
    public static IInferenceSessionManager InferenceSessionManager;
}