using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Nodes.Analyzers;

namespace Furesoft.LowCode.Nodes;

[Description("Executes when a Signal was triggered")]
[GraphAnalyzer(typeof(SignalNodeAnalyzer))]
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
