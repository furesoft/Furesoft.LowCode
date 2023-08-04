using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web;

public class GetTextNode : WebNode, IOutVariableProvider
{
    public string Selector { get; set; } = string.Empty;
    public string OutVariable { get; set; }

    public GetTextNode() : base("GetText")
    {
    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();

        var element = await page.QuerySelectorAsync(Selector);
        SetOutVariable(OutVariable, element.GetPropertyAsync("innerText"));
    }
}
