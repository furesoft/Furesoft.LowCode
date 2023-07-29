using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Furesoft.LowCode.Designer.Core.Components.Views;
using Furesoft.LowCode.Editor.Model;
using NiL.JS.Core;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Data")]
[NodeView(typeof(AssignNodeView))]
[Description("Save a value for later usage")]
public class AssignNode : VisualNode
{


    public AssignNode() : base("Assign Variable")
    {
      
    }


    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The name of the variable")]
    public string Name { get; set; }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The value of the variable")]
    public string Value { get; set; }
    
    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin FlowInput { get; } = null;
    
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutput { get; } = null;

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var jsVar = Context.GetVariable(Name);
        var value = Context.Eval(Value);

        if (jsVar == JSValue.NotExists)
        {
            Context.DefineVariable(Name, true).Assign(value);
        }
        else
        {
            jsVar.Assign(value);
        }

        await ContinueWith(FlowOutput, cancellationToken: cancellationToken);
    }
}
