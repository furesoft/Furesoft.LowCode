namespace Furesoft.LowCode.Nodes.Data.DataTable.Core;

public partial class ColumnDataDefinition : ObservableObject
{
    [ObservableProperty] private string _columnName;
    [ObservableProperty] private bool _isReadOnly;
    [ObservableProperty] private Type _dataType;
}
