using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core.Components.Views;
using NiL.JS.Core;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels.ControlFlow;

[DataContract(IsReference = true)]
[NodeCategory("Control Flow")]
[Description("Repeats an Action X Times")]
[NodeView(typeof(IconNodeView), "m18 12v2a4 4 0 01-4 4H0M4 22 0 18 4 14M0 10V8A4 4 0 014 4h14m-4-4 4 4-4 4")]
public class RepeatNode : InputOutputNode
{
    public string Times { get; set; }


    [Pin("Do", PinAlignment.Right)] public IOutputPin DoPin { get; set; }


    public RepeatNode() : base("Repeat")
    {
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var times = Evaluate<int>(Times);

        for (int i = 0; i < times; i++)
        {
            var context = new Context(Context);
            context.DefineConstant("index", i);

            await ContinueWith(DoPin, context, cancellationToken: cancellationToken);
        }

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
