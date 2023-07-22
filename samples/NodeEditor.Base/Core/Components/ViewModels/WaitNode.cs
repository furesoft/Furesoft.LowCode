using System.Runtime.Serialization;
using System.Threading.Tasks;
using NodeEditor.Model;
using NodeEditorDemo.Core.Components.Views;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(WaitView))]
public class WaitNode : VisualNode
{
    private int _milliseconds;

    public WaitNode() : base("Wait")
    {
    }

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutputPin { get; set; }
    
    [Pin("Flow Input", PinAlignment.Top)]
    public IOutputPin FlowInputPin { get; set; }

    public int Milliseconds
    {
        get => _milliseconds;
        set => SetProperty(ref _milliseconds, value);
    }

    public override async Task Execute()
    {
        await Task.Delay(_milliseconds);
        
        await ContinueWith(FlowOutputPin);
    }
}
