using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new RedisCacheOptions
            {
                Configuration = "localhost:6379"
            };
            using (var cache = new RedisCache(options))
            {
                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                };

                cache.SetString("setstring", "string123", cacheOptions);

                var result = cache.GetString("setstring");

                Console.WriteLine(result);
            }
        }
    }
}
