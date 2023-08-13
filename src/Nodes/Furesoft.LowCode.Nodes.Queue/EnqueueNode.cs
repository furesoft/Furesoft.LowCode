using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes.Queue;

[Description("Add data to a queue for later processing")]
[NodeView(typeof(IconNodeView),
    "M321 236v-43h-64v-64h-42v64h-64v43h64v64h42v-64h64zM428 86v256h-384v-256h384zM428 385c23 0 43-19 43-43l-1-256c0-23-19-42-42-42h-107v-43h-170v43h-107c-24 0-43 19-43 42v256c0 24 19 43 43 43h384z")]
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
