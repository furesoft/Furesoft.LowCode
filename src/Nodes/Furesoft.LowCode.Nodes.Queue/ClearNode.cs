using System.ComponentModel;

namespace Furesoft.LowCode.Nodes.Queue;

[Description("Clear the queue")]
public class ClearNode : QueueBaseNode
{
    public ClearNode() : base("Clear Queue")
    {
    }


    public override Task Invoke(CancellationToken cancellationToken)
    {
        QueueManager.Instance.ClearQueue(Queue);

        return Task.CompletedTask;
    }
}
