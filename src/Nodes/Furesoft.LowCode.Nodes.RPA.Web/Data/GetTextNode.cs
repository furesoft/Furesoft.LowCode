using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web.Data;

[NodeIcon(
    "M0 0 6.0625 0 9 2.9375 9 10.5 0 10.5 0 0ZM1 1 1 9.5 8 9.5 8 4 5 4 5 1 1 1ZM6 1.3125 6 3 7.6875 3 6 1.3125ZM1.5 1.5 4.5 1.5 4.5 2.75 1.5 2.75 1.5 1.5ZM1.5 3.25 4.5 3.25 4.5 4 1.5 4 1.5 3.25ZM1.5 4.5 7.5 4.5 7.5 5.25 1.5 5.25 1.5 4.5ZM1.5 5.75 7.5 5.75 7.5 6.5 1.5 6.5 1.5 5.75ZM1.5 7 7.5 7 7.5 7.75 1.5 7.75 1.5 7ZM1.5 8.25 7.5 8.25 7.5 9 1.5 9 1.5 8.25Z")]
public class GetTextNode : WebNode, IOutVariableProvider
{
    public GetTextNode() : base("Get Text")
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

        var element = await page.QuerySelectorAsync(Selector);
        SetOutVariable(OutVariable, element.GetPropertyAsync("innerText"));
    }
}
