using System.Runtime.Serialization;

namespace Furesoft.LowCode.Nodes.RPA.Web;

public class NewTabNode : WebNode
{
    [DataMember(EmitDefaultValue = false)]
    public string URL { get; set; }

    public NewTabNode() : base("New Tab")
    {

    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = await GetBrowser().NewPageAsync();
        await page.GoToAsync(Evaluate<string>(URL));
        
        DefineConstant(PageVariableName, page);
    }
}
