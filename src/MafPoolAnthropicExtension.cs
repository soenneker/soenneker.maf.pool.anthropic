using System.Threading;
using System.Threading.Tasks;
using Anthropic;
using Microsoft.Agents.AI;
using Soenneker.Maf.Dtos.Options;
using Soenneker.Maf.Pool.Abstract;

namespace Soenneker.Maf.Pool.Anthropic;

/// <summary>
/// Provides Anthropic-specific registration extensions for <see cref="IMafPool"/>, enabling integration via Microsoft Agent Framework.
/// </summary>
public static class MafPoolAnthropicExtension
{
    /// <summary>
    /// Registers an Anthropic model in the agent pool with optional rate/token limits.
    /// </summary>
    public static ValueTask AddAnthropic(this IMafPool pool, string poolId, string key, string modelId, string apiKey,
        int? rps = null, int? rpm = null, int? rpd = null, int? tokensPerDay = null, string? instructions = null,
        CancellationToken cancellationToken = default)
    {
        var options = new MafOptions
        {
            ModelId = modelId,
            ApiKey = apiKey,
            RequestsPerSecond = rps,
            RequestsPerMinute = rpm,
            RequestsPerDay = rpd,
            TokensPerDay = tokensPerDay,
            AgentFactory = (opts, _) =>
            {
                var client = new AnthropicClient { ApiKey = opts.ApiKey! };
                AIAgent agent = client.AsAIAgent(model: opts.ModelId!, name: opts.ModelId, instructions: instructions ?? "You are a helpful assistant.");
                return new ValueTask<AIAgent>(agent);
            }
        };

        return pool.Add(poolId, key, options, cancellationToken);
    }

    /// <summary>
    /// Unregisters an Anthropic model from the agent pool and removes the associated cache entry.
    /// </summary>
    /// <returns>True if the entry existed and was removed; false if it was not present.</returns>
    public static ValueTask<bool> RemoveAnthropic(this IMafPool pool, string poolId, string key, CancellationToken cancellationToken = default)
    {
        return pool.Remove(poolId, key, cancellationToken);
    }
}
