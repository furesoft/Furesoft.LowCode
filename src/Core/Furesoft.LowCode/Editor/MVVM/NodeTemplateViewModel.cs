namespace Furesoft.LowCode.Editor.MVVM;

[ObservableObject]
public partial class NodeTemplateViewModel : INodeTemplate
{
    [ObservableProperty] private bool _isVisible = true;
    [ObservableProperty] private INode _preview;
    [ObservableProperty] private INode _template;
    [ObservableProperty] private string _title;
}
