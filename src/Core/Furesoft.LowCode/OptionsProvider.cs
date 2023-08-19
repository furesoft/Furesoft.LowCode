using Newtonsoft.Json;

namespace Furesoft.LowCode;

public class OptionsProvider
{
    private Dictionary<string, object> _options = new();

    public T Get<T>() where T : new()
    {
        if (_options.TryGetValue(typeof(T).FullName, out var obj))
        {
            return (T)obj;
        }

        var newObj = new T();

        _options.Add(typeof(T).FullName, newObj);
        
        return newObj;
    }

    internal void Open(Stream strm)
    {
        using var reader = new StreamReader(strm);
        var json = reader.ReadToEnd();

        _options = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
    }

    internal void SaveTo(Stream strm)
    {
        using var writer = new StreamWriter(strm);
        var json = JsonConvert.SerializeObject(_options);
        
        writer.Write(json);
    }
}
