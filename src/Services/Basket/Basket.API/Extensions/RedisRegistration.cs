using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Basket.API.Extensions
{
    public static class RedisRegistration
    {
        public static ConnectionMultiplexer ConfigureRedis(this IServiceProvider services, IConfiguration configuration)
        {
            var redisConfiguration = ConfigurationOptions.Parse(configuration["RedisSettings:ConnectionString"], true);
            redisConfiguration.ResolveDns = true;
            return ConnectionMultiplexer.Connect(redisConfiguration);
        }
    }
}