using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core.Components.Views;
using NiL.JS.Core;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[NodeCategory("Data")]
[NodeView(typeof(AssignNodeView))]
[Description("Save a value for later usage")]
public class AssignNode : InputOutputNode
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

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
