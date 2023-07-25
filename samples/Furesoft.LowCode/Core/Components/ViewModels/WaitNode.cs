using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Furesoft.LowCode.Core.Components.Views;
using Furesoft.LowCode.Core.NodeBuilding;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(WaitView))]
[NodeCategory]
[Description("Wait for the specified time")]
public class WaitNode : VisualNode
{
    private int _waitTime;

    public WaitNode() : base("Wait")
    {
    }

    [Browsable(false)]
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutputPin { get; set; }
    
    [Browsable(false)]
    [Pin("Flow Input", PinAlignment.Top)]
    public IOutputPin FlowInputPin { get; set; }

    [Description("The time to wait in milliseconds")]
    public int WaitTime
    {
        get => _waitTime;
        set => SetProperty(ref _waitTime, value);
    }

    public override async Task Execute()
    {
        await Task.Delay(_waitTime);
        
        await ContinueWith(FlowOutputPin);
    }
}
