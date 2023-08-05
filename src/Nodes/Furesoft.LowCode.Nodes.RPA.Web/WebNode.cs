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
        var pageVariable = Context.GetVariable(pageVariableName);

        if (pageVariable.IsUndefined())
        {
            throw new InvalidNodeConnectionException("There is no Node connected that opens a web browser");
        }

        return pageVariable.As<Page>();
    }

    public sealed override async Task Execute(CancellationToken cancellationToken)
    {
        await Invoke(cancellationToken);

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }

    protected abstract Task Invoke(CancellationToken cancellationToken);
}
