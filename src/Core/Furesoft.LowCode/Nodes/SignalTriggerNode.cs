using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode.Nodes;

[Description("Triggers the selected signal with the specified argument")]
[NodeCategory]
public class SignalTriggerNode : InputOutputNode
{
    private string _signal;

    public SignalTriggerNode() : base("Trigger")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Signal
    {
        get => _signal;
        set
        {
            SetProperty(ref _signal, value);

            Description = $"Trigger Signal '{value}'";
        }
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        SignalStorage.Trigger(Signal);

        await ContinueWith(OutputPin, cancellationToken);
    }

    public override void Compile(CodeWriter builder)
    {
        builder.AppendCall("trigger", Signal).AppendSymbol(';');
    }
}
