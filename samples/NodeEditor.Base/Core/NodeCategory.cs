using System.Collections.Generic;
using NodeEditor.Model;
using NodeEditorDemo.ViewModels;

namespace NodeEditorDemo.Core;

public class NodeCategory : ViewModelBase
{
    private IList<INodeTemplate>? _templates;
    public string Name { get; set; }

    public IList<INodeTemplate>? Templates
    {
        get => _templates;
        set => SetProperty(ref _templates, value);
    }

    public override string ToString()
    {
        return Name;
    }
}
