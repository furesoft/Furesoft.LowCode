using System.Collections.Generic;
using Furesoft.LowCode.ViewModels;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core;

public class NodeCategory : ViewModelBase
{
    private IList<INodeTemplate> _templates;
    public string Name { get; set; }

    public IList<INodeTemplate> Templates
    {
        get => _templates;
        set => SetProperty(ref _templates, value);
    }

    public override string ToString()
    {
        return Name;
    }
}
