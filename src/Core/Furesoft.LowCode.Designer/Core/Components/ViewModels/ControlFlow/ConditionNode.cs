using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core.Components.Views;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels.ControlFlow;

[DataContract(IsReference = true)]
[NodeCategory("Control Flow")]
[NodeView(typeof(ConditionView))]
[Description("Change control flow based on condition")]
public class ConditionNode : InputNode
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

    [Pin("True", PinAlignment.Bottom)] public IOutputPin TruePin { get; set; }

    [Pin("False", PinAlignment.Right)] public IOutputPin FalsePin { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var value = Evaluate<bool>(Condition);

        if (value)
        {
            return ContinueWith(TruePin, cancellationToken: cancellationToken);
        }

        return ContinueWith(FalsePin, cancellationToken: cancellationToken);
    }
}
