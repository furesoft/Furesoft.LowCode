using CommunityToolkit.Mvvm.ComponentModel;

namespace Furesoft.LowCode.Editor.MVVM;

[ObservableObject]
public partial class NodeTemplateViewModel : INodeTemplate
{
    [ObservableProperty] private string? _title;
    [ObservableProperty] private INode? _template;
    [ObservableProperty] private INode? _preview;
    [ObservableProperty] private bool _isVisible = true;
}
