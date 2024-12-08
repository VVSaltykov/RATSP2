namespace RATSP.Common.Interfaces;

public interface IRedisService
{
    Task SetAsync(string key, byte[] value);
    Task<byte[]> GetAsync(string key);
}