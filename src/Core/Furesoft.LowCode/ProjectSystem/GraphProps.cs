using CommunityToolkit.Mvvm.Input;
using Furesoft.LowCode.Designer.Layout.Models.Tools.Parameters;

namespace Furesoft.LowCode.ProjectSystem;

public partial class GraphProps : ObservableObject
{
    public ObservableCollection<PropertyDescriptor> Properties { get; set; } = new();
    public ObservableCollection<PinDescriptor> Pins { get; set; } = new();

    [RelayCommand]
    public void AddProperty()
    {
        Properties.Add(new());
    }

    [RelayCommand]
    public void AddPin()
    {
        Pins.Add(new());
    }
}
