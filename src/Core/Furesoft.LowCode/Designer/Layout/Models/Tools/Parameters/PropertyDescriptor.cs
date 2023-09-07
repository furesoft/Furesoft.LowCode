namespace Furesoft.LowCode.Designer.Layout.Models.Tools.Parameters;

public partial class PropertyDescriptor : ObservableObject
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private Type _type;
    [ObservableProperty] private object _value;
}
