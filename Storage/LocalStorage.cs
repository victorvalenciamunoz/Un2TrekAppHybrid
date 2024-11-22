namespace Un2TrekApp.Storage;

internal class LocalStorage : ILocalStorage
{
    public async Task SetAsync(string key, string value)
    {
        await SecureStorage.Default.SetAsync(key, value);
    }
    public async Task<string> GetAsync(string key)
    {
        return await SecureStorage.Default.GetAsync(key);
    }
}
