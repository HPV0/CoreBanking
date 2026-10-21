using CoreBanking.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreBanking.API.Filters.IdempotencyFilter
{
    public class IdempotentFilter(int ttlMinutes = 60)
    : IAsyncActionFilter
    {
        private readonly TimeSpan _ttl = TimeSpan.FromMinutes(ttlMinutes);

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            if (!request.Headers.TryGetValue("Idempotency-Key", out var keyVal) || !Guid.TryParse(keyVal, out var key))
            {
                throw new CustomValidationException("Missing or invalid Idempotency-Key");
            }

            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
            var redis = context.HttpContext.RequestServices.GetRequiredService<IConnectionMultiplexer>().GetDatabase();

            var cacheKey = $"idem:{key}";
            var lockKey = $"lock:{cacheKey}";

            var acquired = await redis.StringSetAsync(lockKey, "1", TimeSpan.FromSeconds(5), When.NotExists);
            if (!acquired)
            {
                throw new CustomValidationException("Another request is processing this key.");
            }

            try
            {
                var existing = await cache.GetStringAsync(cacheKey);
                if (existing != null)
                {

                    var saved = JsonSerializer.Deserialize<IdempotentResponse>(existing)!;
                    context.Result = new ObjectResult(saved.Value) { StatusCode = saved.StatusCode };
                    return;
                }

                var result = await next();

                if (result.Result is ObjectResult { StatusCode: >= 200 and < 300 } obj)
                {
                    var toCache = new IdempotentResponse(obj.StatusCode.Value, obj.Value);

                    var json = JsonSerializer.Serialize(toCache);
                    await cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _ttl
                    });
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                await redis.KeyDeleteAsync(lockKey);
            }
        }
    }
}
