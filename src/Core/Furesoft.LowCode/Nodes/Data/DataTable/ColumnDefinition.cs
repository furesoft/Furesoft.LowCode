namespace Furesoft.LowCode.Nodes.Data.DataTable;

public partial class ColumnDefinition : ObservableObject
{
    [ObservableProperty] private string _columnName;
    [ObservableProperty] private bool _isReadOnly;
    [ObservableProperty] private Type _dataType;
}
