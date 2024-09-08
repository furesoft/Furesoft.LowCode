using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Nodes.IO.Linq;

[NodeCategory("Data/Linq")]
public abstract class LinqNode(string label) : InputOutputNode(label), IOutVariableProvider, IPipeable
{
    [Description("Output Variable")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
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
            return Iterate(p);
        }

        return Array.Empty<object>();
    }

    protected abstract IEnumerable Iterate(IEnumerable src);
}
