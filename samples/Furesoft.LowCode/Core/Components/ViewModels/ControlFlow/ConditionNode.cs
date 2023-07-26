using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Furesoft.LowCode.Core.Components.Views;
using Furesoft.LowCode.Core.NodeBuilding;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core.Components.ViewModels.ControlFlow;

[DataContract(IsReference = true)]
[NodeCategory("Control Flow")]
[NodeView(typeof(ConditionView))]
[Description("Change control flow based on condition")]
public class ConditionNode : VisualNode
{
    private string _condition;

    public ConditionNode() : base("Condition")
    {
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Condition
    {
        get => _condition;
        set => SetProperty(ref _condition, value);
    }
    
    [Pin("Flow", PinAlignment.Top)]
    public IOutputPin FlowPin { get; set; }
    
    [Pin("True", PinAlignment.Bottom)]
    public IOutputPin TruePin { get; set; }
    
    [Pin("False", PinAlignment.Right)]
    public IOutputPin FalsePin { get; set; }

    public override Task Execute()
    {
        var value = Evaluate<bool>(Condition);
      
        if (value)
        {
            return ContinueWith(TruePin);
        }

        return ContinueWith(FalsePin);
    }
}
