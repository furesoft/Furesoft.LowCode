using System.ComponentModel.DataAnnotations;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes.Network;

[Description("Gets the content of a website")]
[NodeView(typeof(IconNodeView),
    "M138 164.5V152C113 152 113 144.5 113 139.5H169.25L175.5 133.25V8.25L169.25 2H6.75L.5 8.25V123.75L13 111.25V14.5H163V127H91.375L97.625 133.25 66.375 164.5H138zM39.75 164.5 8.5 133.25 17.375 124.5 38 145.125V64.5H50.5V145L71 124.375 79.875 133.25 48.625 164.5H39.75z")]
[NodeCategory("Network")]
public class GetSiteContentNode : InputOutputNode, IOutVariableProvider
{
    public GetSiteContentNode() : base("Get Site Content")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string URL { get; set; }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string OutVariable { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var content = await client.GetStringAsync(Evaluate<string>(URL), cancellationToken);

        SetOutVariable(OutVariable, content);

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
