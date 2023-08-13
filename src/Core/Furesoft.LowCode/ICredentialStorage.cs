namespace Furesoft.LowCode;

public interface ICredentialStorage
{
    void Add(string key, object value);
    object Get(string key);
    IEnumerable<string> GetKeys();

    public T Get<T>(string key)
    {
        return (T)Get(key);
    }
}
