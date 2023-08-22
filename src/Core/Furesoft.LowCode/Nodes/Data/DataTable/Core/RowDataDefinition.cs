namespace Furesoft.LowCode.Nodes.Data.DataTable.Core;

public class RowDataDefinition : ObservableObject
{
    private readonly Dictionary<string, object> _properties = new();

    public T GetProperty<T>(string name)
    {
        if (_properties.TryGetValue(name, out var value))
        {
            return (T)value;
        }

        return default;
    }

    public void SetProperty(string name, object value)
    {
        _properties[name] = value;

        this.OnPropertyChanged(name);
    }
}
