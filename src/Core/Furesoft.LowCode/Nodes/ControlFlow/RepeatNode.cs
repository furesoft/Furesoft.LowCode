using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes.ControlFlow;

[NodeCategory("Control Flow")]
[Description("Repeats an Action X Times")]
[NodeIcon("m18 12v2a4 4 0 01-4 4H0M4 22 0 18 4 14M0 10V8A4 4 0 014 4h14m-4-4 4 4-4 4")]
public class RepeatNode() : InputOutputNode("Repeat")
{
    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<int> Times { get; set; }

    [Pin("Do", PinAlignment.Right)] public IOutputPin DoPin { get; set; }


    public override async Task Execute(CancellationToken cancellationToken)
    {
        for (var i = 0; i < Times; i++)
        {
            var context = new Context(Context);
            context.DefineConstant("index", i);

            await ContinueWith(DoPin, cancellationToken, context);
        }

        await ContinueWith(OutputPin, cancellationToken);
    }
}
