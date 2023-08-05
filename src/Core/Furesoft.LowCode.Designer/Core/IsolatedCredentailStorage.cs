using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace Furesoft.LowCode.Designer.Core;

public class IsolatedCredentailStorage : ICredentialStorage, IDisposable
{
    private readonly IsolatedStorageFile _storage;
    private readonly IsolatedStorageFileStream _file;
    private Dictionary<string, object> _data = new();

    public IsolatedCredentailStorage()
    {
        _storage = IsolatedStorageFile.GetMachineStoreForApplication();
        
        _file = _storage.OpenFile("credentials.json", FileMode.OpenOrCreate);
    }

    public void Add(string key, object value)
    {
        var json = JsonConvert.SerializeObject(_data);

        using var writer = new StreamWriter(_file);
        writer.Write(json);
    }

    public object Get(string key)
    {
        _data.TryGetValue(key, out var value);

        return value;
    }

    public IEnumerable<string> GetKeys()
    {
        return _data.Keys;
    }

    public void Dispose()
    {
        _file.Dispose();
        _storage?.Dispose();
    }
}
