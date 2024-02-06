using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrasturcture.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await database.StringGetAsync(basketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task DeleteBasketAsync(string basketId)
            => await database.KeyDeleteAsync(basketId);


        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            return !created ? null : basket;
        }
    }
}
