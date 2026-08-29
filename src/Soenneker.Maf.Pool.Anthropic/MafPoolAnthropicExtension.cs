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
    /// <param name="pool">Pool that supplies the reusable resource.</param>
    /// <param name="poolId">Identifier of the target pool.</param>
    /// <param name="key">Key used to locate the target entry.</param>
    /// <param name="modelId">Identifier of the model to use.</param>
    /// <param name="apiKey">API key used to authenticate the request.</param>
    /// <param name="rps">Optional requests-per-second limit.</param>
    /// <param name="rpm">Optional requests-per-minute limit.</param>
    /// <param name="rpd">Optional requests-per-day limit.</param>
    /// <param name="tokensPerDay">Optional daily token limit.</param>
    /// <param name="instructions">Instructions supplied to the model or processor.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the anthropic addition is complete.</returns>
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
    /// <param name="pool">Pool that supplies the reusable resource.</param>
    /// <param name="poolId">Identifier of the target pool.</param>
    /// <param name="key">Key used to locate the target entry.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>True if the entry existed and was removed; false if it was not present.</returns>
    public static ValueTask<bool> RemoveAnthropic(this IMafPool pool, string poolId, string key, CancellationToken cancellationToken = default)
    {
        return pool.Remove(poolId, key, cancellationToken);
    }
}
