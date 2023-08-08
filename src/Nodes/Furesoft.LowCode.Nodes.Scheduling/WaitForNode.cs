using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes.Scheduling;

[NodeCategory("Scheduling")]
[NodeView(typeof(IconNodeView),
    "M2.688 3.3725c-.2235.1375.5985 2.121.799 2.444.175.2845.5465.371.829.197.284-.176.3725-.547.1975-.83C4.315 4.8595 2.911 3.2345 2.688 3.3725zM2.5205 1.201C2.9845 1.0395 3.4815.95 4 .95s1.0155.0895 1.4795.251c.1645.057.3825-.0575.286-.3055-.0705-.18-.1385-.356-.166-.4275-.0655-.1695-.3-.3095-.402-.3325C4.8115.0485 4.4115 0 4 0S3.1885.0485 2.802.1355C2.7.1585 2.466.2985 2.4005.468 2.373.5395 2.3045.7155 2.2345.8955 2.138 1.1435 2.356 1.2585 2.5205 1.201zM8.549 1.593c-.096-.115-.198-.2275-.3065-.336-.108-.1085-.2205-.21-.335-.3065-.0765-.0645-.3015-.117-.444.0255-.142.1425-.824.8235-.824.8235.201.144.3965.3025.5775.483.181.1805.3385.376.483.5775 0 0 .6815-.681.8235-.8235C8.6665 1.8935 8.614 1.669 8.549 1.593zM4 1.45c-2.2375 0-4.0505 1.813-4.0505 4.05 0 2.2375 1.813 4.0505 4.0505 4.0505 2.2365 0 4.05-1.813 4.05-4.0505C8.05 3.2635 6.2365 1.45 4 1.45zM4 8.5505c-1.684 0-3.05-1.3655-3.05-3.05 0-1.6845 1.3655-3.05 3.05-3.05 1.6845 0 3.0505 1.3655 3.0505 3.05C7.0505 7.1845 5.6845 8.5505 4 8.5505z",
    MinWidth = 90, MinHeight = 90)]
[Description("Cancel Action After Specific Amount Of Time")]
public class WaitForNode : InputOutputNode
{
    public WaitForNode() : base("Wait For")
    {
    }

    [Pin("Do", PinAlignment.Right)] public IOutputPin DoNode { get; } = null;

    [Description("The time to wait in milliseconds")]
    [DataMember(EmitDefaultValue = false)]
    [Required]
    public int WaitTime { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var cts = new CancellationTokenSource();

        await Task.WhenAny(ContinueWith(DoNode, cancellationToken: cancellationToken), Task.Delay(WaitTime, cts.Token));

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
