namespace Un2TrekApp.Storage;

internal interface ILocalStorage
{
    Task SetAsync(string key, string value);
    Task<string> GetAsync(string key);
}
