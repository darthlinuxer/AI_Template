# AI_Template

A small template workspace with a couple of example tools used for demos and development.

Contents
- `console_log.txt` — sample runtime log file (example output used during development).
- `tools/console.cs` — a simple C# console source file aimed to capture console inputs sent by hooks.
- `tools/treasure-finder/server.cs` — a minimal ModelContextProtocol (MCP) server implemented as a single C# source file. It exposes a demonstration `FindTreasureInLocation` tool.

Quick start

1. Inspect the sample log (if present):

```bash
cat console_log.txt
```

2. Run the treasure-finder MCP server (requires .NET 10):

```bash
dotnet run --project tools/treasure-finder
```

Notes
- The `tools/console.cs` file is a compact C# console example included for reference.
- The `server.cs` file under `tools/treasure-finder` uses `ModelContextProtocol.AspNetCore` to host an MCP server over stdio and is intended as a minimal demonstration rather than production-ready code.

If you'd like, I can:
- Add a more detailed example invocation for an MCP client that talks to the server.
- Add a `README` section for building, testing, or containerizing the projects.
