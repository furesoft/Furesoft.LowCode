using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Furesoft.LowCode.Core.Components.Views;
using Furesoft.LowCode.Core.NodeBuilding;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(WaitView))]
public class WaitNode : VisualNode
{
    private int _milliseconds;

    public WaitNode() : base("Wait")
    {
    }

    [Browsable(false)]
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutputPin { get; set; }
    
    [Browsable(false)]
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
