using Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<string?> GetAsync(string Key) 
        {
            var value =await _database.StringGetAsync(Key);

            return !value.IsNullOrEmpty ? value : default;
        }

        public async Task SetAsync(string Key, object Value, TimeSpan duration)
        {
            var redisValue =JsonSerializer.Serialize(Value);
            await _database.StringSetAsync(Key, redisValue, duration);
        }
    }
}
