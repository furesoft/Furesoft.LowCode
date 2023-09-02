using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.Designer.Layout.Models.Tools.Parameters;

namespace Furesoft.LowCode.Designer.Layout.ViewModels.Tools;

public partial class ParametersToolViewModel : Tool
{
    public ParametersToolViewModel()
    {
        Id = "Parameters";
        Title = Id;
    }

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
