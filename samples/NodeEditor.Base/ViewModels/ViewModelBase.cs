using CommunityToolkit.Mvvm.ComponentModel;
using NodeEditor.Model;

namespace NodeEditorDemo.ViewModels;

[ObservableObject]
public partial class ViewModelBase
{
    public IDrawingNode Drawing { get; set; }
}
