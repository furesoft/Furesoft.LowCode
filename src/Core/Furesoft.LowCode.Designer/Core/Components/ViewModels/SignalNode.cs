using System.ComponentModel;
using System.Runtime.Serialization;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[Description("Executes when a Signal was triggered")]
[NodeCategory]
public class SignalNode : OutputNode
{
    public SignalNode() : base("Signal")
    {

    }

    [DataMember(EmitDefaultValue = false)]
    public string Signal { get; set; }


    public override async Task Execute(CancellationToken cancellationToken)
    {
        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
