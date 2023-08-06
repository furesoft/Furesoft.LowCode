using Dock.Model.Mvvm.Controls;

namespace Furesoft.LowCode.Designer.Layout.ViewModels.Documents;

public partial class DocumentViewModel : Document
{
    [ObservableProperty] private EditorViewModel _editor;

    public DocumentViewModel(NodeFactory nodeFactory)
    {
        _editor = new();

        _editor.Serializer = new NodeSerializer(typeof(ObservableCollection<>));
        _editor.Factory = nodeFactory;
        _editor.Templates = _editor.Factory.CreateTemplates();
        _editor.Drawing = _editor.Factory.CreateDrawing();
        _editor.Drawing.SetSerializer(_editor.Serializer);
    }
}
