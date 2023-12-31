namespace Furesoft.LowCode.Editor.Model;

public interface INodeSerializer
{
    string Serialize<T>(T value);
    T Deserialize<T>(string text);
}
