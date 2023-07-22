using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NodeEditor.Model;

namespace Furesoft.LowCode.ViewModels;

[ObservableObject]
public partial class ViewModelBase
{
    [Browsable(false)]
    public IDrawingNode Drawing { get; set; }
}
