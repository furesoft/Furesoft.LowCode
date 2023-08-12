using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web.Analyzers;

public class WebBrowserAnalyzer : GraphAnalyzerBase<WebNode>
{
    public override void Analyze(WebNode node)
    {
        if (node is OpenBrowserNode)
        {
            return;
        }

        var isBrowserConnectedBefore = AnalyzerContext.IsInputConnected<OpenBrowserNode>();

        if (!isBrowserConnectedBefore)
        {
            AddError("OpenBrowser node is not connected");
        }
    }
}
