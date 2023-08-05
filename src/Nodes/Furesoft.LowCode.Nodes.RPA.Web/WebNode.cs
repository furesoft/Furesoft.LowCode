using Furesoft.LowCode.Designer.Core;
using Furesoft.LowCode.Nodes.RPA.Web.Core;
using NiL.JS.Extensions;
using PuppeteerSharp;

namespace Furesoft.LowCode.Nodes.RPA.Web;

[NodeCategory("Automation/Web")]
public abstract class WebNode : InputOutputNode
{
    protected const string PageVariableName = "browserPage";
    protected const string BrowserVariableName = "browser";

    protected WebNode(string label) : base(label)
    {
    }

    protected Page GetPage()
    {
        var pageVariable = Context.GetVariable(PageVariableName);

        if (pageVariable.IsUndefined())
        {
            throw new InvalidNodeConnectionException("There is no Node connected that opens a web browser");
        }

        return pageVariable.As<Page>();
    }
    
    protected IBrowser GetBrowser()
    {
        var browserVariable = Context.GetVariable(BrowserVariableName);

        if (browserVariable.IsUndefined())
        {
            throw new InvalidNodeConnectionException("There is no Node connected that opens a web browser");
        }

        var browser =  browserVariable.As<IBrowser>();

        if (browser.IsClosed || !browser.IsConnected)
        {
            throw new BrowserNotStartedException();
        }

        return browser;
    }

    public sealed override async Task Execute(CancellationToken cancellationToken)
    {
        await Invoke(cancellationToken);

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }

    protected abstract Task Invoke(CancellationToken cancellationToken);
}
