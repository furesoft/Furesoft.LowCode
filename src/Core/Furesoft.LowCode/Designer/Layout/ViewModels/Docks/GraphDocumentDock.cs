using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.Designer.Layout.ViewModels.Documents;

namespace Furesoft.LowCode.Designer.Layout.ViewModels.Docks;

public class GraphDocumentDock : DocumentDock
{
    private readonly NodeFactory _factory;

    public GraphDocumentDock(NodeFactory factory)
    {
        _factory = factory;
        CreateDocument = new RelayCommand(CreateNewDocument);
    }

    private void CreateNewDocument()
    {
        if (!CanCreateDocument)
        {
            return;
        }

        var index = VisibleDockables?.Count + 1;
        var document = new GraphDocumentViewModel(_factory) {Id = $"Document{index}", Title = $"Document{index}"};

        Factory?.AddDockable(this, document);
        Factory?.SetActiveDockable(document);
        Factory?.SetFocusedDockable(this, document);
    }
}
