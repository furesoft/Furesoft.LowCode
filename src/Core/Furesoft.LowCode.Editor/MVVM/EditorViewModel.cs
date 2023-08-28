using CommunityToolkit.Mvvm.ComponentModel;

namespace Furesoft.LowCode.Editor.MVVM;

[ObservableObject]
public partial class EditorViewModel : INodeTemplatesHost, IEditor
{
    [ObservableProperty] private IDrawingNode _drawing;
    [ObservableProperty] private INodeFactory _factory;
    [ObservableProperty] private INodeSerializer _serializer;
    [ObservableProperty] private IList<INodeTemplate> _templates;

    public void Load(string content)
    {
        _drawing = _serializer.Deserialize<IDrawingNode>(content);
    }
}
