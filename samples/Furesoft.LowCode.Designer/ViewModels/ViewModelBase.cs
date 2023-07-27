using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Designer.ViewModels;

[ObservableObject]
public partial class ViewModelBase
{
    [Browsable(false)]
    public IDrawingNode Drawing { get; set; }
}
