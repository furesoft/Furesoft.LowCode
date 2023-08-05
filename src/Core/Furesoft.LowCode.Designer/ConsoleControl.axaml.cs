using System.Text;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Furesoft.LowCode.Designer;

[TemplatePart(Name = "PART_Output")]
[TemplatePart(Name = "PART_Input")]
[TemplatePart(Name = "PART_Submit")]
public class ConsoleControl : TemplatedControl
{
    private Button _btn;
    private TextBox _inputBox;
    private TextBlock _outputBlock;
    private ScrollViewer _scrollViewer;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _outputBlock = e.NameScope.Find<TextBlock>("PART_Output");
        _inputBox = e.NameScope.Find<TextBox>("PART_Input");
        _btn = e.NameScope.Find<Button>("PART_Submit");
        _scrollViewer = e.NameScope.Find<ScrollViewer>("PART_ScrollViewer");

        Console.SetOut(new Writer(_outputBlock, _scrollViewer));
        Console.SetIn(new Reader(_inputBox, _btn));
    }

    public class Reader : TextReader
    {
        private readonly TextBox _inputBox;
        private readonly Button _button;
        private AutoResetEvent _mrs;
        private string _inputText;

        public Reader(TextBox inputBox, Button button)
        {
            _inputBox = inputBox;
            _button = button;
            _mrs = new(false);
            _button.Click += ButtonOnClick;
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            _inputText = _inputBox.Text;
            _mrs.Set();
            _inputBox.Text = "";
            _inputBox.IsEnabled = false;
        }

        public override string ReadLine()
        {
            Dispatcher.UIThread.Post(() =>
            {
                _inputBox.IsEnabled = true;
            });
            
            _mrs.WaitOne();

            return _inputText;
        }
    }

    public class Writer : TextWriter
    {
        private readonly TextBlock _output;
        private readonly ScrollViewer _scrollViewer;

        public Writer(TextBlock output, ScrollViewer scrollViewer)
        {
            _output = output;
            _scrollViewer = scrollViewer;
        }

        public override Encoding Encoding { get; }

        public override void Write(char value)
        {
            Dispatcher.UIThread.Post(() =>
            {
                _output.Text += value;
                _scrollViewer.ScrollToEnd();
            });
        }
    }
}
