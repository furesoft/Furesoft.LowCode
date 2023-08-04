using Furesoft.LowCode.Designer.Core;
using NiL.JS.Extensions;
using PuppeteerSharp;

namespace Furesoft.LowCode.Nodes.RPA.Web;

[NodeCategory("Automation/Web")]
public abstract class WebNode : InputOutputNode
{
    protected string pageVariableName = "browserPage";

    protected WebNode(string label) : base(label)
    {
    }

    protected Page GetPage()
    {
        return Context.GetVariable(pageVariableName).As<Page>();
    }

    public sealed override async Task Execute(CancellationToken cancellationToken)
    {
        await Invoke(cancellationToken);
        
        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }

    protected abstract Task Invoke(CancellationToken cancellationToken);
}
