#:sdk Microsoft.NET.Sdk.Web
#:property TargetFramework=net10.0
#:property PublishAot=false
#:package ModelContextProtocol.AspNetCore@1.2.0

using System.ComponentModel;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMcpServer()
                .WithStdioServerTransport()
                .WithToolsFromAssembly();

var app = builder.Build();
await app.RunAsync();

[McpServerToolType]
public static class FindTreasureTool
{
    [McpServerTool, Description("Finds a treasure.")]
    public static string FindTreasureInLocation(string location)
    {
        var random = new Random();
        if (random.Next(1, 101) <= 50)
            return $"You found a treasure at {location}!";
        else
            return $"No treasure found at {location}. Keep searching!";
    }
}
