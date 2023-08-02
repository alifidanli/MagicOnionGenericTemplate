﻿using MagicT.Redis.Options;
using Microsoft.Extensions.DependencyInjection;

namespace MagicT.Redis;

/// <summary>
/// Provides rate limiting functionality using Redis as the data store.
/// </summary>
public class RateLimiter
{
    private readonly MagicTRedisDatabase MagicTRedisDatabase;

    private readonly RateLimiterConfig RateLimiterConfig;

    public IServiceProvider Provider { get; set; }

    /// <summary>
    /// Initializes a new instance of the RateLimiter class using dependency injection.
    /// </summary>
    /// <param name="provider">The service provider used for dependency injection.</param>
    public RateLimiter(IServiceProvider provider)
    {
        MagicTRedisDatabase = provider.GetService<MagicTRedisDatabase>();
        RateLimiterConfig = provider.GetService<RateLimiterConfig>();
    }

    /// <summary>
    /// Checks if the client has exceeded the rate limit and increments the request count.
    /// </summary>
    /// <param name="clientIp">The unique identifier of the client.</param>
    /// <param name="requestLimit">The maximum number of requests allowed within the time window.</param>
    /// <param name="timeWindow">The duration of the time window.</param>
    /// <returns>Returns true if the client is within the rate limit; otherwise, false.</returns>
    public bool CheckRateLimit(string clientId)
    {
        // Generate the Redis key for the client's rate limit.
        string redisKey = $"RateLimit:{clientId}";

        // Get the current count of requests for the client from Redis.
        int currentCount = (int)MagicTRedisDatabase.MagicTRedisDb.StringGet(redisKey);

        // Check if the client has exceeded the rate limit.
        if (currentCount >= RateLimiterConfig.RateLimit)
        {
            return false;
        }

        // Increment the request count for the client within the current time window.
        // Note that the INCR command in Redis is atomic, ensuring thread safety.
        // The time window will be enforced by the Redis key expiration.
        MagicTRedisDatabase.MagicTRedisDb.StringIncrement(redisKey);

        // Set the expiration time for the Redis key to match the time window.
        MagicTRedisDatabase.MagicTRedisDb.KeyExpire(redisKey, TimeSpan.FromMinutes(RateLimiterConfig.TimeWindowMinutes));

        return true;
    }
}
