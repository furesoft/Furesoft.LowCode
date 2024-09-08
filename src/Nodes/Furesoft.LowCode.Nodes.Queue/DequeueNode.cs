using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Furesoft.LowCode.Nodes.Queue;

[Description("Get data from queue")]
[NodeIcon(
    "M322 237v-43h-170v43h170zM429 87v256h-384v-256h384zM429 386c23 0 43-19 43-43l-1-256c0-23-19-42-42-42h-107v-43h-170v43h-107c-24 0-43 19-43 42v256c0 24 19 43 43 43h384z")]
public class DequeueNode() : QueueBaseNode("Get Data From Queue"), IOutVariableProvider
{
    [Required] [DataMember] public string OutVariable { get; set; }

    public override Task Invoke(CancellationToken cancellationToken)
    {
        var value = QueueManager.Instance.Dequeue<object>(Queue);
        SetOutVariable(OutVariable, value);

        return Task.CompletedTask;
    }
}
