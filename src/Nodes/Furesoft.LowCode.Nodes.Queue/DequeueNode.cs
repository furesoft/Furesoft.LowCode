using System.ComponentModel;
using System.Runtime.Serialization;

namespace Furesoft.LowCode.Nodes.Queue;

[Description("Get data from queue")]
public class DequeueNode : QueueBaseNode, IOutVariableProvider
{
    public DequeueNode() : base("Get From Queue")
    {
    }

    [DataMember] public string OutVariable { get; set; }


    public override Task Invoke(CancellationToken cancellationToken)
    {
        var value = QueueManager.Instance.Dequeue<object>(Queue);
        SetOutVariable(OutVariable, value);

        return Task.CompletedTask;
    }
}
