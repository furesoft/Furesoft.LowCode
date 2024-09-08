using System.Collections;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Linq;

[NodeIcon(
    "M10.8125 20.3125V23H18.8125C20.25 23 21.5 21.75 21.5 20.3125V12.3125H18.8125V20.3125H10.8125ZM26.8125 7H21.5V4.3125H24.1875L20.1875.3125 16.1875 4.3125H18.8125V7H8.1875C6.75 7 5.5 8.25 5.5 9.6875V20.3125H.1875V23H5.5V25.6875H2.8125L6.8125 29.6875 10.8125 25.6875H8.1875V9.6875H26.8125V7Z")]
public class TransformNode() : LinqNode("Transform")
{
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public Evaluatable<object> Transformer { get; set; }

    protected override IEnumerable Iterate(IEnumerable src)
    {
        foreach (var pi in src)
        {
            DefineConstant("$", pi);

            yield return Transformer;
        }
    }
}
