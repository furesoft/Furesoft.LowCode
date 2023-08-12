using System.ComponentModel;
using System.Runtime.Serialization;

namespace Furesoft.LowCode.Nodes.Queue;

[Description("Add data to a queue for later processing")]
public class EnqueueNode : QueueBaseNode
{
    public EnqueueNode() : base("Add To Queue")
    {
    }

    [DataMember] public Evaluatable<object> Data { get; set; }


    public override Task Invoke(CancellationToken cancellationToken)
    {
        QueueManager.Instance.Enqueue(Queue, Evaluate(Data));

        return Task.CompletedTask;
    }
}
