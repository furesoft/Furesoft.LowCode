using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode.Nodes.Scheduling;

[NodeCategory("Scheduling")]
[Description("Continue ellapsed every interval")]
public class TimerNode : InputOutputNode
{
    private double _interval;

    public TimerNode() : base("Timer")
    {
    }

    [Pin("Ellapsed", PinAlignment.Right)] public IOutputPin EllapsedPin { get; set; }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public double Interval
    {
        get => _interval;
        set
        {
            SetProperty(ref _interval, value);

            Description = $"Continue ellapsed every {value} milliseconds";
        }
    }

    public override void Compile(CodeWriter builder)
    {
        var callbackWriter = new CodeWriter();
        CompilePin(OutputPin, callbackWriter);

        var callback = "function (arg) {\n\t" + callbackWriter + "\n}\n";
        builder.AppendCall("setInterval", Interval, callback.AsEvaluatable()).AppendSymbol(';').AppendSymbol('\n');
    }
}
