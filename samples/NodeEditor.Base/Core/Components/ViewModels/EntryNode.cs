using System.Runtime.Serialization;
using System.Threading.Tasks;
using NodeEditor.Model;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[IgnoreTemplate]
[NodeView(typeof(EntryView))]
public class EntryNode : VisualNode
{
    public EntryNode() : base("Entry")
    {
    }

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowPin { get; set; }

    public override Task Execute()
    {
       return ExecutePin(FlowPin);
    }
}
