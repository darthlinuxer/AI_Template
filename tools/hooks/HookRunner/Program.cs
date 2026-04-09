using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

if (args.Length == 0)
{
    await Console.Error.WriteLineAsync("Missing hook event name argument.");
    return 1;
}

var eventName = args[0];

try
{
    var data = await ReadInputAsync();
    var details = BuildDetails(eventName, data);
    WriteLog(eventName, data, details);
    return 0;
}
catch (JsonException)
{
    await Console.Error.WriteLineAsync("Invalid hook payload.");
    return 1;
}
catch (ArgumentException ex)
{
    await Console.Error.WriteLineAsync(ex.Message);
    return 1;
}
catch (Exception ex)
{
    await Console.Error.WriteLineAsync(ex.Message);
    return 1;
}

static async Task<JsonObject> ReadInputAsync()
{
    var input = await Console.In.ReadToEndAsync();
    return string.IsNullOrWhiteSpace(input)
        ? new JsonObject()
        : JsonNode.Parse(input)?.AsObject() ?? new JsonObject();
}

static string BuildDetails(string eventName, JsonObject data)
{
    return eventName switch
    {
        "SessionStart" => $"source={GetString(data, "source") ?? "n/a"}",
        "UserPromptSubmit" => $"prompt={NormalizePrompt(GetString(data, "prompt"))}",
        "PreToolUse" => $"tool={GetString(data, "tool_name") ?? "unknown"}",
        "PostToolUse" => $"tool={GetString(data, "tool_name") ?? "unknown"}",
        "PreCompact" => $"trigger={GetString(data, "trigger") ?? "n/a"}",
        "SubagentStart" => $"agent={GetString(data, "agent_type") ?? "unknown"} | id={GetString(data, "agent_id") ?? "n/a"}",
        "SubagentStop" => $"agent={GetString(data, "agent_type") ?? "unknown"} | id={GetString(data, "agent_id") ?? "n/a"}",
        "Stop" => $"stop_hook_active={GetString(data, "stop_hook_active") ?? "n/a"}",
        _ => throw new ArgumentException($"Unsupported hook event name: {eventName}"),
    };
}

static string NormalizePrompt(string? prompt)
{
    var normalized = Regex.Replace(prompt ?? string.Empty, @"\s+", " ").Trim();
    if (normalized.Length > 80)
    {
        normalized = normalized[..80];
    }

    return string.IsNullOrWhiteSpace(normalized) ? "n/a" : normalized;
}

static void WriteLog(string eventName, JsonObject data, string details)
{
    var logPath = GetLogPath(data);
    var directoryPath = Path.GetDirectoryName(logPath);

    if (!string.IsNullOrWhiteSpace(directoryPath))
    {
        Directory.CreateDirectory(directoryPath);
    }

    var line = $"[{NextIndex(logPath)}] {eventName} | {details} | time={GetString(data, "timestamp") ?? DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture)}{Environment.NewLine}";
    File.AppendAllText(logPath, line, Encoding.UTF8);
}

static string GetLogPath(JsonObject data)
{
    var cwd = GetString(data, "cwd");
    return Path.Combine(string.IsNullOrWhiteSpace(cwd) ? Directory.GetCurrentDirectory() : cwd, "pirate-ship.log");
}

static int NextIndex(string logPath)
{
    return !File.Exists(logPath)
        ? 1
        : File.ReadLines(logPath).Count(static line => !string.IsNullOrWhiteSpace(line)) + 1;
}

static string? GetString(JsonObject data, string propertyName)
{
    if (!data.TryGetPropertyValue(propertyName, out var node) || node is null)
    {
        return null;
    }

    if (node is JsonValue value)
    {
        if (value.TryGetValue<string>(out var stringValue))
        {
            return stringValue;
        }

        if (value.TryGetValue<bool>(out var boolValue))
        {
            return boolValue ? "true" : "false";
        }

        if (value.TryGetValue<long>(out var longValue))
        {
            return longValue.ToString(CultureInfo.InvariantCulture);
        }

        if (value.TryGetValue<double>(out var doubleValue))
        {
            return doubleValue.ToString(CultureInfo.InvariantCulture);
        }

        if (value.TryGetValue<decimal>(out var decimalValue))
        {
            return decimalValue.ToString(CultureInfo.InvariantCulture);
        }
    }

    return node.ToJsonString();
}