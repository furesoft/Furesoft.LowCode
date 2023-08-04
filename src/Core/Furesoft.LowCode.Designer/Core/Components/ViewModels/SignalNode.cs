using System.ComponentModel;
using System.Runtime.Serialization;
using Avalonia.PropertyGrid.Model.Collections;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[Description("Executes when a Signal was triggered")]
[DataContract(IsReference = true)]
[NodeCategory]
public class SignalNode : OutputNode
{
    public SignalNode() : base("Signal")
    {

    }

    public string Signal { get; set; }


    public override async Task Execute(CancellationToken cancellationToken)
    {
        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
