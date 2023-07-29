using CommunityToolkit.Mvvm.ComponentModel;

namespace Furesoft.LowCode.Editor.MVVM;

[ObservableObject]
public partial class EditorViewModel : INodeTemplatesHost, IEditor
{
    [ObservableProperty] private INodeSerializer? _serializer;
    [ObservableProperty] private INodeFactory? _factory;
    [ObservableProperty] private IList<INodeTemplate>? _templates;
    [ObservableProperty] private IDrawingNode? _drawing;
}
