using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web.Interaction;

[NodeIcon(
    "M1 16H5.24a1 1 0 00.71-.29l6.92-6.93h0L15.71 6a1 1 0 000-1.42L11.47.29a1 1 0 00-1.42 0L7.23 3.12h0L.29 10.05a1 1 0 00-.29.71V15A1 1 0 001 16ZM10.76 2.41l2.83 2.83L12.17 6.66 9.34 3.83ZM2 11.17l5.93-5.93 2.83 2.83L4.83 14H2Za1 1 0 000 2Z")]
public class TypeIntoNode : WebNode
{
    public TypeIntoNode() : base("Type Into")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Selector { get; set; } = string.Empty;


    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> Text { get; set; }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();

        var element = await page.QuerySelectorAsync(Selector);

        await element.TypeAsync(Text);
    }
}
