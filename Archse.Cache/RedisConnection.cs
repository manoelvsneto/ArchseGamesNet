using Microsoft.Extensions.Caching.Distributed;

namespace Archse.Cache
{

    public class RedisConnection
    {

        private readonly IDistributedCache _distributedCache;

        public RedisConnection(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public string GetValueFromKey(string key)
        {
            return _distributedCache.GetString(key);
        }
        public bool SetValueFromKey(string key, string value)
        {
            _distributedCache.SetString(key, value);
            return true;
        }
    }
}