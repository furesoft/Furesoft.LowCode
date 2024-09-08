using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Analyzers;

namespace Furesoft.LowCode.Nodes;

[Description("Executes when a Signal was triggered")]
[GraphAnalyzer(typeof(SignalNodeAnalyzer))]
[NodeCategory]
[NodeIcon(
    "M10 8C8.9 8 8 8.9 8 10S8.9 12 10 12 12 11.1 12 10 11.1 8 10 8M16 10C16 6.7 13.3 4 10 4S4 6.7 4 10C4 12.2 5.2 14.1 7 15.2L8 13.5C6.8 12.8 6 11.5 6 10.1 6 7.9 7.8 6.1 10 6.1S14 7.9 14 10.1C14 11.6 13.2 12.9 12 13.5L13 15.2C14.8 14.2 16 12.2 16 10M10 0C4.5 0 0 4.5 0 10 0 13.7 2 16.9 5 18.6L6 16.9C3.6 15.5 2 12.9 2 10 2 5.6 5.6 2 10 2S18 5.6 18 10C18 13 16.4 15.5 14 16.9L15 18.6C18 16.9 20 13.7 20 10 20 4.5 15.5 0 10 0Z")]
public class SignalNode() : OutputNode("Signal")
{
    private string _signal;

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
