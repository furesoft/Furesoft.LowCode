using System.Collections;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Linq;

[NodeCategory("Data/LINQ")]
[NodeIcon(
    "M10.8125 20.3125V23H18.8125C20.25 23 21.5 21.75 21.5 20.3125V12.3125H18.8125V20.3125H10.8125ZM26.8125 7H21.5V4.3125H24.1875L20.1875.3125 16.1875 4.3125H18.8125V7H8.1875C6.75 7 5.5 8.25 5.5 9.6875V20.3125H.1875V23H5.5V25.6875H2.8125L6.8125 29.6875 10.8125 25.6875H8.1875V9.6875H26.8125V7Z")]
public class TransformNode : InputOutputNode, IOutVariableProvider, IPipeable
{
    public TransformNode() : base("Transform")
    {
    }

    public Evaluatable<object> Transformer { get; set; }

    public string OutVariable { get; set; }
    public object PipeVariable { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        ApplyPipe<IEnumerable>();

        var result = LazyIteratePipeVariable();

        if (!string.IsNullOrEmpty(OutVariable))
        {
            SetOutVariable(OutVariable, result);
        }

        PipeVariable = result;

        return ContinueWith(OutputPin, cancellationToken);
    }

    public IEnumerable LazyIteratePipeVariable()
    {
        if (PipeVariable is IEnumerable p)
        {
            foreach (var pi in p)
            {
                DefineConstant("$", pi);

                yield return Transformer;
            }
        }
    }
}
