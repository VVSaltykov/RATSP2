﻿using RATSP.Common.Interfaces;
using StackExchange.Redis;

namespace RATSP.Common.Services;

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task SetAsync(string key, byte[] value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(key, value);
    }

    public async Task<byte[]> GetAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return await db.StringGetAsync(key);
    }
}