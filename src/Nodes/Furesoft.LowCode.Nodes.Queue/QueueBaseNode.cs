using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Nodes.Queue;

[NodeCategory("Data/Queue")]
[Description("Add Data To A Queue For Later Proccessing")]
public abstract class QueueBaseNode : InputOutputNode
{
    private string _queue;

    protected QueueBaseNode(string label) : base(label)
    {
        ShowDescription = true;
    }

    [DataMember]
    [Required]
    public string Queue
    {
        get => _queue;
        set
        {
            _queue = value;
            Description = Label + " " + value;
        }
    }

    public sealed override async Task Execute(CancellationToken cancellationToken)
    {
        await Invoke(cancellationToken);

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }

    public abstract Task Invoke(CancellationToken cancellationToken);
}
