using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Analyzers;

namespace Furesoft.LowCode.Nodes;

[Description("Executes when a Signal was triggered")]
[GraphAnalyzer(typeof(SignalNodeAnalyzer))]
[NodeCategory]
public class SignalNode : OutputNode
{
    private string _signal;

    public SignalNode() : base("Signal")
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

            Description = $"On '{value}'";
        }
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await ContinueWith(OutputPin, cancellationToken);
    }
}
