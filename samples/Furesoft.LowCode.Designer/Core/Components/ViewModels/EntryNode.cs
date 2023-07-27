using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Furesoft.LowCode.Designer.Core.Components.Views;
using Furesoft.LowCode.Designer.Core.NodeBuilding;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[IgnoreTemplate]
[NodeView(typeof(EntryView))]
[Description("The starting node of the graph")]
public class EntryNode : VisualNode
{
    public EntryNode() : base("Entry")
    {
    }
    
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowPin { get; set; }

    public override Task Execute()
    {
        return ContinueWith(FlowPin);
    }
}
