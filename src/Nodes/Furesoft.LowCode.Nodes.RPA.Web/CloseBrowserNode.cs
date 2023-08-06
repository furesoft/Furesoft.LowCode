using Furesoft.LowCode.Nodes.RPA.Web.Core;
using NiL.JS.Core;

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
