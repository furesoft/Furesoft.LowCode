using System.ComponentModel;
using System.Runtime.Serialization;
using Avalonia.PropertyGrid.Model.Collections;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[Description("Triggers the selected signal with the specified argument")]
[NodeCategory]
public class SignalTriggerNode : InputOutputNode
{
    public SignalTriggerNode() : base("Trigger")
    {

    }

    [DataMember(EmitDefaultValue = false)]
    public string Signal { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _evaluator.Signals.Trigger(Signal, token: cancellationToken);

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
