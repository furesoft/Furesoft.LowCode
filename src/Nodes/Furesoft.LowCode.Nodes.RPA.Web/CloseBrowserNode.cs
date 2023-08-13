using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web;

public class CloseBrowserNode : WebNode
{
    public CloseBrowserNode() : base("Close Browser")
    {
    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        await GetBrowser().CloseAsync();

        DeleteConstant(BrowserVariableName);
    }
}
