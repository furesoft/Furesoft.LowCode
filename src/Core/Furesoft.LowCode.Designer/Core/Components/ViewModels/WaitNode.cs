using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core.Components.Views;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(IconNodeView),
    "M0 0H12V6H12V6L8 10 12 14V14H12V20H0V14H0V14L4 10 0 6V6H0V0M10 14.5 6 10.5 2 14.5V18H10V14.5M6 9.5 10 5.5V2H2V5.5L6 9.5M4 4H8V4.75L6 6.75 4 4.75V4Z")]
[NodeCategory]
[Description("Wait for the specified time")]
public class WaitNode : InputOutputNode
{
    private int _waitTime;

    public WaitNode() : base("Wait")
    {
    }

    [Description("The time to wait in milliseconds")]
    public int WaitTime
    {
        get => _waitTime;
        set => SetProperty(ref _waitTime, value);
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await Task.Delay(_waitTime, cancellationToken);

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
