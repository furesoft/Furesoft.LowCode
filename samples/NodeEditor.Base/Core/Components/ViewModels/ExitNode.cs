using System.Runtime.Serialization;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[IgnoreTemplate]
public class ExitNode : VisualNode
{
    public ExitNode() : base("Exit")
    {
    }

    [Pin("Flow Input")]
    public IInputPin FlowPin { get; set; }
}
