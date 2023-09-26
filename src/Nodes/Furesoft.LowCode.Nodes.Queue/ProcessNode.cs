using System.ComponentModel;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes.Queue;

[Description("Process items in a queue")]
[NodeIcon(
    "M0 13H3M0 8H3M19 13H22M19 8H22M14 19V22M8 19V22M14 0V3M8 0V3M8 8H14V14H8ZM5 3H17C18.108 3 19 3.892 19 5V17C19 18.108 18.108 19 17 19H5C3.892 19 3 18.108 3 17V5C3 3.892 3.892 3 5 3Z")]
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

            await ContinueWith(DoPin, cancellationToken, subContext);
        }
    }

    public override void Compile(CodeWriter builder)
    {
        var callbackWriter = new CodeWriter();
        CompilePin(DoPin, callbackWriter);

        var callback = "function (item) {\n\t" + callbackWriter + "\n}\n";
        builder.AppendCall("processQueue", Queue, callback.AsEvaluatable()).AppendSymbol(';').AppendSymbol('\n');

        CompilePin(OutputPin, builder);
    }
}
