using System.Runtime.Serialization;
using NodeEditor.Model;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[IgnoreTemplate]
[NodeView(typeof(DefaultNodeView))]
public class EntryNode : VisualNode
{
    public EntryNode() : base("Entry")
    {
    }

    [Pin("Flow Output", PinAlignment.Right)]
    public IOutputPin FlowPin { get; set; }
}
