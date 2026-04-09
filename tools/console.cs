#: sdk Microsoft.NET.Sdk
#: property TargetFramework=net10.0
#: property PublishAot=false

using System.Text.Json;

string logPath = "console_log.txt";
string input = Console.In.ReadToEnd();

using var doc = JsonDocument.Parse(input);

var root = doc.RootElement;
var filtered = new Dictionary<string, object>();

if (root.TryGetProperty("timestamp", out var timestamp))
    filtered["timestamp"] = timestamp.GetString() ?? string.Empty;

if (root.TryGetProperty("hook_event_name", out var hookEventName))
    filtered["hook_event_name"] = hookEventName.GetString() ?? string.Empty;

if (root.TryGetProperty("tool_name", out var toolName))
    filtered["tool_name"] = toolName.GetString() ?? string.Empty;

if (root.TryGetProperty("prompt", out var prompt))
    filtered["prompt"] = prompt.GetString() ?? string.Empty;

if (root.TryGetProperty("tool_input", out var toolInput))
    filtered["tool_input"] = toolInput;

if (root.TryGetProperty("tool_response", out var toolResponse))
    filtered["tool_response"] = toolResponse;

input = JsonSerializer.Serialize(filtered, new JsonSerializerOptions { WriteIndented = true });


var contentToAppend = input.EndsWith(Environment.NewLine, StringComparison.Ordinal)
    ? input
    : input + Environment.NewLine;
File.AppendAllText(logPath, contentToAppend);

