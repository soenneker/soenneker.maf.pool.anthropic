[![](https://img.shields.io/nuget/v/soenneker.maf.pool.anthropic.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.maf.pool.anthropic/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.maf.pool.anthropic/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.maf.pool.anthropic/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.maf.pool.anthropic.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.maf.pool.anthropic/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.maf.pool.anthropic/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.maf.pool.anthropic/actions/workflows/codeql.yml)

# Soenneker.Maf.Pool.Anthropic

Provides Anthropic-specific registration extensions for `IMafPool`, enabling integration via Microsoft Agent Framework.

## Install

```bash
dotnet add package Soenneker.Maf.Pool.Anthropic
```

## Usage

```csharp
using Soenneker.Maf.Pool.Anthropic;
using Soenneker.Maf.Pool.Abstract;

await pool.AddAnthropic(
    poolId: "chat",
    key: "claude-primary",
    modelId: "claude-sonnet-4-5",
    apiKey: configuration["ANTHROPIC_API_KEY"]!,
    rpm: 60,
    instructions: "Answer concisely.",
    cancellationToken: cancellationToken);

(AIAgent? agent, IMafPoolEntry? entry) =
    await pool.GetAvailable("chat", cancellationToken);
```

`pool` is an `IMafPool` registered by `Soenneker.Maf.Pool`. The agent is created lazily on its first successful checkout and then reused for the entry.

## What you get

- `MafPoolAnthropicExtension` — Provides Anthropic-specific registration extensions for `IMafPool`, enabling integration via Microsoft Agent Framework.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `MafPoolAnthropicExtension.AddAnthropic(pool, poolId, key, modelId, apiKey, rps, rpm, rpd, tokensPerDay, instructions, cancellationToken)` | Registers an Anthropic model in the agent pool with optional rate/token limits. | A task that completes when the anthropic addition is complete. |
| `MafPoolAnthropicExtension.RemoveAnthropic(pool, poolId, key, cancellationToken)` | Unregisters an Anthropic model from the agent pool and removes the associated cache entry. | True if the entry existed and was removed; false if it was not present. |

## Practical notes

- Store the API key in a secret provider; the pool retains it in the entry options while the entry is registered.
- Omitted instructions default to `You are a helpful assistant.`
- Checkout consumes one request from the configured quota. `tokensPerDay` is also charged once per checkout; this adapter does not reconcile actual provider token usage.
- Call `RemoveAnthropic()` to unregister the entry and evict its cached agent.
