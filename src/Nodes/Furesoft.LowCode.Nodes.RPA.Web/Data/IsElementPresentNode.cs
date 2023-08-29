using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web.Data;

[NodeIcon(
    "M9 5A4 4 90 015 9 4 4 90 011 5 4 4 90 015 1C5.38 1 5.75 1.055 6.1 1.155L6.885.37C6.305.13 5.67 0 5 0A5 5 90 000 5 5 5 90 005 10 5 5 90 0010 5M2.955 4.04 2.25 4.75 4.5 7 9.5 2 8.795 1.29 4.5 5.585 2.955 4.04Z")]
public class IsElementPresentNode : WebNode, IOutVariableProvider
{
    public IsElementPresentNode() : base("Is Element Present")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Selector { get; set; } = string.Empty;

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string OutVariable { get; set; }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();

        var element = await page.QuerySelectorAllAsync(Selector);

        SetOutVariable(OutVariable, element.Any());
    }
}
