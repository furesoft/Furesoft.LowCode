using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.RPA.Web.Analyzers;
using NiL.JS.Extensions;
using PuppeteerSharp;

namespace Furesoft.LowCode.Nodes.RPA.Web.Core;

[NodeCategory("Automation/Web")]
[GraphAnalyzer(typeof(WebBrowserAnalyzer))]
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
            throw CreateError("There is no Node connected that opens a web browser");
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

        var browser = browserVariable.As<IBrowser>();

        if (browser.IsClosed || !browser.IsConnected)
        {
            throw new BrowserNotStartedException();
        }

        return browser;
    }
}
