using System.Dynamic;

namespace Furesoft.LowCode.Nodes.Data.DataTable;

public partial class ColumnDataDefinition : ObservableObject
{
    [ObservableProperty] private string _columnName;
    [ObservableProperty] private bool _isReadOnly;
    [ObservableProperty] private Type _dataType;
}


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
