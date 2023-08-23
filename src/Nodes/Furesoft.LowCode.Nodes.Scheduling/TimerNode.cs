using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Timer = System.Timers.Timer;

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

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var timer = new Timer();
        timer.Interval = Interval;
        timer.Elapsed += async (sender, args) =>
        {
            await ContinueWith(EllapsedPin, cancellationToken);
        };
        timer.Start();

        await ContinueWith(OutputPin, cancellationToken);
    }
}
