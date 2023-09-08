using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes.ControlFlow;

[NodeCategory("Control Flow")]
[NodeView(typeof(ConditionView))]
[Description("Change control flow based on condition")]
public class ConditionNode : InputNode
{
    private Evaluatable<bool> _condition;

    public ConditionNode() : base("Condition")
    {
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Required]
    public Evaluatable<bool> Condition
    {
        get => _condition;
        set
        {
            SetProperty(ref _condition, value);
            Description = $"Continue with True if condition '{value.Source}' is true otherwise continue with False";
        }
    }

    [Pin("True", PinAlignment.Bottom)] public IOutputPin TruePin { get; set; }

    [Pin("False", PinAlignment.Right)] public IOutputPin FalsePin { get; set; }

    public override void Compile(CodeWriter builder)
    {
        builder.AppendStatementHead("if", Condition.Source);
        CompilePin(TruePin, builder);
        builder.EndBlock();

        if (HasConnection(FalsePin))
        {
            builder.AppendKeyword("else");
            builder.BeginBlock();
            CompilePin(FalsePin, builder);
            builder.EndBlock();
        }
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        if (Condition)
        {
            return ContinueWith(TruePin, cancellationToken);
        }

        return ContinueWith(FalsePin, cancellationToken);
    }
}
