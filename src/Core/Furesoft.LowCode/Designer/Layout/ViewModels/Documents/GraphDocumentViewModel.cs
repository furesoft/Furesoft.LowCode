using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.Designer.Services.Serializing;
using Furesoft.LowCode.ProjectSystem.Items;

namespace Furesoft.LowCode.Designer.Layout.ViewModels.Documents;

public partial class GraphDocumentViewModel : Document
{
    [ObservableProperty] private EditorViewModel _editor;
    [ObservableProperty] private GraphProps _props;

    public GraphDocumentViewModel(NodeFactory nodeFactory, GraphItem item)
    {
        _editor = new();

        _editor.Serializer = new NodeSerializer(typeof(ObservableCollection<>));
        _editor.Factory = nodeFactory;
        _editor.Templates = new(_editor.Factory.CreateTemplates());

        _editor.Drawing = item.Drawing ?? _editor.Factory.CreateDrawing();

        _editor.Drawing.SetSerializer(_editor.Serializer);
        _editor.Drawing.Name = item.Name;


        item.Props ??= new();
        _props = item.Props;

        Title = item.Name;
        Id = item.Id;
    }
}
