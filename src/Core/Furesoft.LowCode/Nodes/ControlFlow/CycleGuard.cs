using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.NodeViews;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Nodes.ControlFlow;

[Description("Breaks an endless cycle by counting the iterations")]
[NodeCategory("Control Flow")]
[NodeView(typeof(IconNodeView), "M12 0C8.249 0 3.725.861 0 2.755 0 6.845-.051 17.037 12 24 24.051 17.037 24 6.845 24 2.755 20.275.861 15.751 0 12 0zm-.106 15.429L6.857 9.612c.331-.239 1.75-1.143 2.794.042l2.187 2.588c.009-.001 5.801-5.948 5.815-5.938.246-.22.694-.503 1.204-.101l-6.963 9.226z")]
public class CycleGuard : InputOutputNode
{
    public CycleGuard() : base("Cycle Guard")
    {
    }

    [Description("Defines how many iterations are allowed")]
    [DataMember(EmitDefaultValue = false)]
    public int MaxIterations { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var counterName = "cycle_guard_counter_" + ID;
        
        if (Context.GetVariable(counterName) == JSValue.NotExists)
        {
            DefineConstant(counterName, 1);
        }

        var counter = Context.GetVariable(counterName).As<int>();

        if (counter < MaxIterations)
        {
            Context.GetVariable(counterName).Assign(++counter);
            
            await ContinueWith(OutputPin, cancellationToken);
        }
    }
}
