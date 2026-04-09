# AI_Template

A small template workspace with a couple of example tools used for demos and development.

Contents
- `tools/hooks/HookRunner` — A small .NET console tool that accepts a hook event name as the first argument and reads an optional JSON payload from stdin. It writes simple, append-only logs to `pirate-ship.log` in the current working directory.
- `tools/treasure-finder` — A minimal ModelContextProtocol (MCP) server implemented as a single C# source file (`server.cs`). It exposes a toy `FindTreasureInLocation` tool for demonstration.
- `AI_Template.sln` — solution file referencing the projects in the workspace.

Quick start

1. Run the HookRunner (from repository root):

```bash
dotnet run --project tools/hooks/HookRunner -- SessionStart
# Provide JSON payload on stdin if needed, e.g.:
# echo '{"source":"cli","timestamp":"2026-01-01T00:00:00Z"}' | dotnet run --project tools/hooks/HookRunner -- SessionStart
```

2. Run the treasure-finder MCP server (requires .NET 10):

```bash
dotnet run --project tools/treasure-finder
```

Notes
- The `HookRunner` project is a compact console app that normalizes and logs hook events; useful for testing hook integrations and generating deterministic logs.
- The `server.cs` file under `tools/treasure-finder` uses `ModelContextProtocol.AspNetCore` to host an MCP server over stdio. It is intended as a minimal example, not production-ready code.

If you'd like, I can:
- Add a more detailed example invocation for the MCP client that talks to the server.
- Add a `README` section for building, testing, or containerizing the projects.
