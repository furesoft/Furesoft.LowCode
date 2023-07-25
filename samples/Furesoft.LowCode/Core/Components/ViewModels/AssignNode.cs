using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Furesoft.LowCode.Core.NodeBuilding;
using NiL.JS.Core;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Data")]
[Description("Save a value for later usage")]
public class AssignNode : VisualNode
{
    private string _name;
    private string _value;

    public AssignNode() : base("Assign Variable")
    {
        _name = string.Empty;
        _value = string.Empty;
    }


    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The name of the variable")]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The value of the variable")]
    public string Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }
    
    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin FlowInput { get; } = null;
    
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutput { get; } = null;

    public override async Task Execute()
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

        await ContinueWith(FlowOutput);
    }
}
