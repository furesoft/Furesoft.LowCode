using System.Runtime.Serialization;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Value")]
public class GetCurrentDateTimeNode : VisualNode
{
    public GetCurrentDateTimeNode() : base("CurrentDateTime")
    {
    }

    [Pin("Date")]
    public IInputPin DatePin { get; set; }

    [Pin("Time")]
    public IInputPin TimePin { get; set; }
}
