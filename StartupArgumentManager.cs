using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
    
    FileLocksmith,

    LanFileShare
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
        var actionArg = args.FirstOrDefault(e => e.StartsWith("-action:", StringComparison.OrdinalIgnoreCase));
        var valueArgIndex = Array.FindIndex(args, e => e.StartsWith("-value:", StringComparison.OrdinalIgnoreCase));
        var valueArg = valueArgIndex >= 0 ? args[valueArgIndex] : null;

        if (!string.IsNullOrEmpty(actionArg))
        {
            var actionStr = actionArg.Substring("-action:".Length);
            if (Enum.TryParse(actionStr, true, out StartupAction action))
            {
                result.Action = action;
                result.Value = valueArg?.Substring("-value:".Length).Trim('"') ?? string.Empty;

                if (result.Action == StartupAction.LanFileShare)
                {
                    var filePaths = ParseLanShareFilePaths(args, valueArgIndex);
                    result.Value = PackValues(filePaths);
                }
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

    private static IReadOnlyList<string> ParseLanShareFilePaths(IReadOnlyList<string> args, int valueArgIndex)
    {
        if (valueArgIndex < 0 || valueArgIndex >= args.Count)
        {
            return [];
        }

        var values = new List<string>();

        for (var i = valueArgIndex; i < args.Count; i++)
        {
            var token = args[i];
            if (string.IsNullOrWhiteSpace(token))
            {
                continue;
            }

            if (i == valueArgIndex)
            {
                token = token.Substring("-value:".Length);
            }
            else if (token.StartsWith("-", StringComparison.Ordinal))
            {
                break;
            }

            token = token.Trim().Trim('"');
            if (string.IsNullOrWhiteSpace(token))
            {
                continue;
            }

            if (string.Equals(token, "{all}", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(token, "%*", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(token, "{0}", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(token, "%1", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            values.Add(token);
        }

        return values
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
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

    public static string PackValues(IEnumerable<string> values)
    {
        var normalized = values
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => v.Trim().Trim('"'))
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return JsonSerializer.Serialize(normalized);
    }

    public static IReadOnlyList<string> UnpackValues(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        var trimmed = value.Trim();

        if (trimmed.StartsWith("[", StringComparison.Ordinal))
        {
            try
            {
                var values = JsonSerializer.Deserialize<List<string>>(trimmed);
                if (values != null)
                {
                    return values
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .Select(v => v.Trim().Trim('"'))
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }
            }
            catch
            {
            }
        }

        return [trimmed.Trim('"')];
    }
}
