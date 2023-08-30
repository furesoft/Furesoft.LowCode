﻿using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.Designer.Services.Serializing;
using Furesoft.LowCode.ProjectSystem;

namespace Furesoft.LowCode.Designer.Layout.ViewModels.Documents;

public partial class GraphDocumentViewModel : Document
{
    [ObservableProperty] private EditorViewModel _editor;

    public GraphDocumentViewModel(NodeFactory nodeFactory, GraphItem item)
    {
        _editor = new();

        _editor.Serializer = new NodeSerializer(typeof(ObservableCollection<>));
        _editor.Factory = nodeFactory;
        _editor.Templates = new(_editor.Factory.CreateTemplates());
        _editor.Drawing = _editor.Factory.CreateDrawing();
        _editor.Drawing.SetSerializer(_editor.Serializer);
        _editor.Drawing.Name = item.Name;
        Title = item.Name;
        Id = item.Id;
    }
}
