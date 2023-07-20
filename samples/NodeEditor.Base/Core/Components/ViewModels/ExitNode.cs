using System.Runtime.Serialization;
using NodeEditor.Model;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[IgnoreTemplate]
public class ExitNode : VisualNode
{
    public ExitNode() : base("Exit")
    {
    }

    [Pin("Flow Input", PinAlignment.Left)]
    public IInputPin FlowPin { get; set; }
}
