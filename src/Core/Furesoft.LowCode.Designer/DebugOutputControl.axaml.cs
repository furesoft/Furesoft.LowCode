using System.Text;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Furesoft.LowCode.Nodes.IO;

namespace Furesoft.LowCode.Designer;

public class DebugOutputControl : TemplatedControl
{
    public static readonly StyledProperty<string> OutputTextProperty =
        AvaloniaProperty.Register<DebugOutputControl, string>(nameof(OutputText));

    public string OutputText
    {
        get => GetValue(OutputTextProperty);
        set => SetValue(OutputTextProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        DebugOutNode.OutputWriter = new DebugOutputWriter(this);
        
        base.OnApplyTemplate(e);
    }
}

public class DebugOutputWriter : TextWriter
{
    private readonly DebugOutputControl _debugOutputControl;

    public DebugOutputWriter(DebugOutputControl debugOutputControl)
    {
        _debugOutputControl = debugOutputControl;
    }

    public override Encoding Encoding { get; }

    public override void Write(char value)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            var currentText = _debugOutputControl.GetValue(DebugOutputControl.OutputTextProperty);
            currentText += value;

            _debugOutputControl.SetValue(DebugOutputControl.OutputTextProperty, currentText);
        });
    }
}
