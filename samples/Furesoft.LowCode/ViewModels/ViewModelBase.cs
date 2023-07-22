using CommunityToolkit.Mvvm.ComponentModel;
using NodeEditor.Model;

namespace Furesoft.LowCode.ViewModels;

[ObservableObject]
public partial class ViewModelBase
{
    public IDrawingNode Drawing { get; set; }
}
