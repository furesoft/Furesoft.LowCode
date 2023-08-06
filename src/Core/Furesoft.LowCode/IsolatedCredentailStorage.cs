using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace Furesoft.LowCode;

public class IsolatedCredentailStorage : ICredentialStorage, IDisposable
{
    private readonly IsolatedStorageFile _storage;
    private Dictionary<string, object> _data = new();

    public IsolatedCredentailStorage()
    {
        _storage = IsolatedStorageFile.GetMachineStoreForApplication();
        
        var file = _storage.OpenFile("credentials.json", FileMode.OpenOrCreate);
        _data = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(file).ReadToEnd());
        file.Dispose();
    }

    public void Add(string key, object value)
    {
        var json = JsonConvert.SerializeObject(_data);

        using var writer = new StreamWriter(_storage.OpenFile("credentials.json", FileMode.Create));
        writer.Write(json);
    }

    public object Get(string key)
    {
        _data.TryGetValue(key, out var value);

        return value;
    }

    public IEnumerable<string> GetKeys()
    {
        if (_data == null)
        {
            return Array.Empty<string>();
        }
        
        return _data?.Keys;
    }

    public void Dispose()
    {
        _storage?.Dispose();
    }
}
