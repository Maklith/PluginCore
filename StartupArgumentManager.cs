using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginCore;

public enum StartupAction
{
    None,
    RepeatStartup,
    
    // Path Operations
    IndexAdd,
    IndexRemove,
    IndexCheck,
    
    PinAdd,
    PinRemove,
    PinCheck,
    
    // Plugin Operations
    PluginAdd,
    PluginRemove,
    PluginCheck,
    
    DownloadPlugin,
    
    FileLocksmith
}

public class StartupResult
{
    public StartupAction Action { get; set; }
    public string Value { get; set; }
    public Dictionary<string, string> Extras { get; set; } = new();
}

public static class StartupArgumentManager
{
    public static StartupResult Parse(string[]? args)
    {
        if (args == null || args.Length == 0)
        {
            return new StartupResult { Action = StartupAction.RepeatStartup };
        }

        // Check for URL Protocol
        if (args.Length == 1 && args[0].StartsWith("kitopiaurl://", StringComparison.OrdinalIgnoreCase))
        {
            return ParseUrl(args[0]);
        }

        var result = new StartupResult();
        var actionArg = args.FirstOrDefault(e => e.StartsWith("-action:"));
        var valueArg = args.FirstOrDefault(e => e.StartsWith("-value:"));

        if (!string.IsNullOrEmpty(actionArg))
        {
            var actionStr = actionArg.Substring("-action:".Length);
            if (Enum.TryParse(actionStr, true, out StartupAction action))
            {
                result.Action = action;
                result.Value = valueArg?.Substring("-value:".Length).Trim('"') ?? string.Empty;
            }
        }
        else
        {
            // Try to detect legacy URL passed as a regular argument string if "kitopiaurl://" wasn't at the very start (e.g. wrapped in quotes or handled differently by OS)
            // But usually the check above covers it.
            // If we have random args but no action, it's None or RepeatStartup? 
            // Default to None if args exist but unknown.
            result.Action = StartupAction.None;
        }
        
        return result;
    }

    private static StartupResult ParseUrl(string url)
    {
        var result = new StartupResult();
        var content = url.Replace("kitopiaurl://", "", StringComparison.OrdinalIgnoreCase).TrimEnd('/');
        
        // Handle semicolon separated k=v pairs
        var parts = content.Split(';');
        
        foreach (var part in parts)
        {
            var kv = part.Split('=');
            if (kv.Length != 2) continue;
            
            var key = kv[0].Trim();
            var val = kv[1].Trim();
            
            if (key.Equals("action", StringComparison.OrdinalIgnoreCase))
            {
                 if (Enum.TryParse(val, true, out StartupAction act)) result.Action = act;
            }
            else if (key.Equals("value", StringComparison.OrdinalIgnoreCase))
            {
                result.Value = val;
            }
            
            result.Extras[key] = val;
        }
        
        // Legacy Inference
        if (result.Action == StartupAction.None)
        {
            if (result.Extras.ContainsKey("pluginId") && result.Extras.ContainsKey("pluginVersionInt"))
            {
                result.Action = StartupAction.DownloadPlugin;
            }
            else if (result.Extras.Count > 0) 
            {
                // If we have data but no action, assume we just pass it along (maybe for simple open?)
                // Or map to RepeatStartup with data?
                // Let's leave as None, but Extras are populated.
            }
        }

        return result;
    }

    public static string GenerateCmd(StartupAction action, string value)
    {
        return $"-action:{action} -value:\"{value}\"";
    }

    public static string GenerateUrl(StartupAction action, string value)
    {
        return $"kitopiaurl://action={action};value={value}";
    }
}
