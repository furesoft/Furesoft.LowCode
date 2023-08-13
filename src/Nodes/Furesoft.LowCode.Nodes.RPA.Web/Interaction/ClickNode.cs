using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web.Interaction;

public class ClickNode : WebNode
{
    public ClickNode() : base("Click")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Selector { get; set; } = string.Empty;

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();

        var element = await page.QuerySelectorAsync(Selector);

        await element.ClickAsync();
    }
}
