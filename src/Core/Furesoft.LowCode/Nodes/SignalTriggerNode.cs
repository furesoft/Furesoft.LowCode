using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Furesoft.LowCode.Nodes;

[Description("Triggers the selected signal with the specified argument")]
[NodeCategory]
public class SignalTriggerNode : InputOutputNode
{
    public SignalTriggerNode() : base("Trigger")
    {

    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Signal { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _evaluator.Signals.Trigger(Signal, token: cancellationToken);

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
