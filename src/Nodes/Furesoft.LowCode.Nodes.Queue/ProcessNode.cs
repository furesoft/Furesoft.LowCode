using System.ComponentModel;
using Furesoft.LowCode.Attributes;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes.Queue;

[Description("Process items in a queue")]
public class ProcessNode : QueueBaseNode
{
    public ProcessNode() : base("Process Queue")
    {
    }

    [Pin("Do", PinAlignment.Right)] public IOutputPin DoPin { get; set; }

    public override async Task Invoke(CancellationToken cancellationToken)
    {
        while (QueueManager.Instance.GetItemCountInQueue(Queue) > 0)
        {
            var subContext = new Context(Context);
            subContext.DefineConstant("item",
                Context.GlobalContext.WrapValue(QueueManager.Instance.Dequeue<object>(Queue)));

            await ContinueWith(DoPin, subContext, cancellationToken);
        }
    }
}
