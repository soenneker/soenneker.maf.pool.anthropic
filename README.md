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

## Quick start

```csharp
using Soenneker.Maf.Pool.Anthropic;

IMafPool pool = /* obtain from your application */;
await pool.AddAnthropic("value", "value", "value", "value", default);
```

Registers an Anthropic model in the agent pool with optional rate/token limits.

## What you get

- `MafPoolAnthropicExtension` — Provides Anthropic-specific registration extensions for `IMafPool`, enabling integration via Microsoft Agent Framework.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `MafPoolAnthropicExtension.AddAnthropic(pool, poolId, key, modelId, apiKey, rps, rpm, rpd, tokensPerDay, instructions, cancellationToken)` | Registers an Anthropic model in the agent pool with optional rate/token limits. | A task that completes when the anthropic addition is complete. |
| `MafPoolAnthropicExtension.RemoveAnthropic(pool, poolId, key, cancellationToken)` | Unregisters an Anthropic model from the agent pool and removes the associated cache entry. | True if the entry existed and was removed; false if it was not present. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
