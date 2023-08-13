using System.Text;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;

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
        _inputBox.KeyDown += InputBoxOnKeyDown;

        Console.SetOut(new Writer(_outputBlock, _scrollViewer));
        Console.SetIn(new Reader(_inputBox, _btn));
    }

    private void InputBoxOnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _btn.Command.Execute(null);
        }
    }

    public class Reader : TextReader
    {
        private readonly Button _button;
        private readonly TextBox _inputBox;
        private string _inputText;
        private readonly AutoResetEvent _mrs;

        public Reader(TextBox inputBox, Button button)
        {
            _inputBox = inputBox;
            _button = button;
            _mrs = new(false);
            _button.Command = new RelayCommand(ButtonOnClick);
        }

        private void ButtonOnClick()
        {
            _inputText = _inputBox.Text;
            _mrs.Set();
            _inputBox.Text = "";
            _inputBox.IsEnabled = false;
            _button.IsEnabled = _inputBox.IsEnabled;
        }

        public override string ReadLine()
        {
            Dispatcher.UIThread.Post(() =>
            {
                _inputBox.IsEnabled = true;
                _button.IsEnabled = _inputBox.IsEnabled;
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
