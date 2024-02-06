using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrasturcture.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response is null)
                return;
            var option = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var serializedResponse = JsonSerializer.Serialize(response, option);

            await database.StringSetAsync(cacheKey, serializedResponse, timeToLive);

        }

        public async Task<string> GetCachedResponse(string cacheKey)
        {
            var cachedResponse = await database.StringGetAsync(cacheKey);

            return !cachedResponse.HasValue ? null : cachedResponse.ToString();
        }

    }
}
