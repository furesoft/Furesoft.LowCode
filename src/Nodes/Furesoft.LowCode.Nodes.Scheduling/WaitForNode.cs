using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Nodes.Scheduling;

[NodeCategory("Scheduling")]
[NodeIcon(
    "M3.638 3.5725c-.2235.1375.5985 2.121.799 2.444.175.2845.5465.371.829.197.284-.176.3725-.547.1975-.83C5.265 5.0595 3.861 3.4345 3.638 3.5725zM3.4705 1.401C3.9345 1.2395 4.4315 1.15 4.95 1.15s1.0155.0895 1.4795.251c.1645.057.3825-.0575.286-.3055-.0705-.18-.1385-.356-.166-.4275-.0655-.1695-.3-.3095-.402-.3325C5.7615.2485 5.3615.2 4.95.2S4.1385.2485 3.752.3355C3.65.3585 3.416.4985 3.3505.668 3.323.7395 3.2545.9155 3.1845 1.0955 3.088 1.3435 3.306 1.4585 3.4705 1.401zM9.499 1.793c-.096-.115-.198-.2275-.3065-.336-.108-.1085-.2205-.21-.335-.3065-.0765-.0645-.3015-.117-.444.0255-.142.1425-.824.8235-.824.8235.201.144.3965.3025.5775.483.181.1805.3385.376.483.5775 0 0 .6815-.681.8235-.8235C9.6165 2.0935 9.564 1.869 9.499 1.793zM4.95 1.65c-2.2375 0-4.0505 1.813-4.0505 4.05 0 2.2375 1.813 4.0505 4.0505 4.0505 2.2365 0 4.05-1.813 4.05-4.0505C9 3.4635 7.1865 1.65 4.95 1.65zM4.95 8.7505c-1.684 0-3.05-1.3655-3.05-3.05 0-1.6845 1.3655-3.05 3.05-3.05 1.6845 0 3.0505 1.3655 3.0505 3.05C8.0005 7.3845 6.6345 8.7505 4.95 8.7505z",
    MinWidth = 90, MinHeight = 90)]
[Description("Cancel Action After Specific Amount Of Time")]
public class WaitForNode : InputOutputNode
{
    private int _waitTime;

    public WaitForNode() : base("Wait For")
    {
    }

    [Pin("Do", PinAlignment.Right)] public IOutputPin DoNode { get; }

    [Description("The time to wait in milliseconds")]
    [DataMember(EmitDefaultValue = false)]
    [Required]
    public int WaitTime
    {
        get => _waitTime;
        set
        {
            SetProperty(ref _waitTime, value);

            Description = $"Cancel Action After {value} milliseconds";
        }
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var cts = new CancellationTokenSource();

        await Task.WhenAny(ContinueWith(DoNode, cancellationToken), Task.Delay(WaitTime, cts.Token));

        await ContinueWith(OutputPin, cancellationToken);
    }
}
