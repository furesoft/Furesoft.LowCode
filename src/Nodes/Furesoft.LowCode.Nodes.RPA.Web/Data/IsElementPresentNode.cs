using System.Runtime.Serialization;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web.Data;

public class IsElementPresentNode : WebNode, IOutVariableProvider
{
    [DataMember(EmitDefaultValue = false)]
    public string Selector { get; set; } = string.Empty;

    [DataMember(EmitDefaultValue = false)]
    public string OutVariable { get; set; }

    public IsElementPresentNode() : base("Is Element Present")
    {
    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();

        var element = await page.QuerySelectorAllAsync(Selector);

        SetOutVariable(OutVariable, element.Any());
    }
}
