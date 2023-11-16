using Application.Contracts.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IConfiguration _configuration;
    private IDatabase _db;
    public CacheService(IConfiguration configuration)
    {
        _configuration = configuration;

        ConfigureRedis();
    }

    private void ConfigureRedis()
    {
        var redisUrl = _configuration.GetValue<string>("Redis:Url");
        if (!string.IsNullOrEmpty(redisUrl))
        {
            var redis = ConnectionMultiplexer.Connect(redisUrl);
            _db = redis.GetDatabase();
        }
    }

    public T GetData<T>(string key)
    {
        var value = _db.StringGet(key);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        return default;
    }
    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
        return isSet;
    }
    public object RemoveData(string key)
    {
        bool _isKeyExist = _db.KeyExists(key);
        if (_isKeyExist == true)
        {
            return _db.KeyDelete(key);
        }
        return false;
    }
}
