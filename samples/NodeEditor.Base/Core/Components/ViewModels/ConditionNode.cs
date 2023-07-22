using System.Runtime.Serialization;
using System.Threading.Tasks;
using NodeEditor.Model;
using NodeEditorDemo.Core.Components.Views;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Value")]
[NodeView(typeof(ConditionView))]
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
      //  bool result = (bool)Evaluator.Evaluate(Condition);
      
        if (true)
        {
            return ContinueWith(TruePin);
        }

        return ContinueWith(FalsePin);
    }
}
