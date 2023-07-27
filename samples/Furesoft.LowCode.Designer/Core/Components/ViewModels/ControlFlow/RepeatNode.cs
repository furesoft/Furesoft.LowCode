using System.ComponentModel;
using System.Threading.Tasks;
using Furesoft.LowCode.Designer.Core.Components.Views;
using Furesoft.LowCode.Designer.Core.NodeBuilding;
using Furesoft.LowCode.Editor.Model;
using NiL.JS.Core;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels.ControlFlow;

[NodeCategory("Control Flow")]
[Description("Repeats an Action X Times")]
[NodeView(typeof(IconNodeView), "m18 12v2a4 4 0 01-4 4H0M4 22 0 18 4 14M0 10V8A4 4 0 014 4h14m-4-4 4 4-4 4")]
public class RepeatNode : VisualNode
{
    public string Times { get; set; }

    [Pin("Flow", PinAlignment.Top)]
    public IInputPin FlowPin { get; set; }
    
    [Pin("Do", PinAlignment.Right)]
    public IOutputPin DoPin { get; set; }
    
    [Pin("Flow", PinAlignment.Bottom)]
    public IOutputPin FlowOutput { get; set; }

    public RepeatNode() : base("Repeat")
    {
    }

    public override async Task Execute()
    {
        var times = Evaluate<int>(Times);

        for (int i = 0; i < times; i++)
        {
            //ToDo: give node index
            var context = new Context(Context);
            context.DefineConstant("i", i);
            
            await ContinueWith(DoPin, context);
        }

        await ContinueWith(FlowOutput);
    }
}
