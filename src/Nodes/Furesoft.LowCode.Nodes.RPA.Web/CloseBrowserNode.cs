using Furesoft.LowCode.Nodes.RPA.Web.Core;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes.RPA.Web;

[NodeView(typeof(IconNodeView),
    "M0 0H16V16H0V0ZM1.5 1.5V14.5H14.5V1.5H1.5ZM6.9393 8 4.6412 5.702 5.7019 4.6413 7.9999 6.9394 10.2981 4.6412 11.3587 5.7019 9.0606 8 11.3587 10.2981 10.298 11.3587 7.9999 9.0607 5.7019 11.3587 4.6412 10.2981 6.9393 8Z")]
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
