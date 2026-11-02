using CoreBanking.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace CoreBanking.API.Filters.IdempotencyFilter
{
    public class IdempotentFilter(int ttlMinutes = 60)
    : IAsyncActionFilter
    {
        private readonly TimeSpan _ttl = TimeSpan.FromMinutes(ttlMinutes);

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            if (!request.Headers.TryGetValue("Idempotency-Key", out var keyVal))
            {
                request.Headers.TryAdd("Idempotency-Key", Guid.NewGuid().ToString());
                request.Headers.TryGetValue("Idempotency-Key", out keyVal);
            }

            if (!Guid.TryParse(keyVal, out var key))
            {
                throw new CustomValidationException("Invalid Idempotency key");
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
                    throw new CustomValidationException("Request with this idempotency key is already processed.");
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
