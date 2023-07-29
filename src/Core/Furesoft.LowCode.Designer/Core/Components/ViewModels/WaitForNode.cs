using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core.Components.Views;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory]
[NodeView(typeof(AssignNodeView))]
[Description("Cancel Action After Specific Amount Of Time")]
public class WaitForNode : VisualNode
{
    public WaitForNode() : base("Wait For")
    {
      
    }

    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin FlowInput { get; } = null;
    
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutput { get; } = null;
    
    [Pin("Do Output", PinAlignment.Right)]
    public IOutputPin DoNode { get; } = null;

    [Description("The time to wait in milliseconds")]
    public int WaitTime { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var cts = new CancellationTokenSource();
        
        await Task.WhenAny(ContinueWith(DoNode, cancellationToken: cancellationToken), Task.Delay(WaitTime, cts.Token));

        await ContinueWith(FlowOutput, cancellationToken: cancellationToken);
    }
}
