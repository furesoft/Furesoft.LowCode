using Avalonia.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;
using AvaloniaEdit.TextMate;
using Furesoft.LowCode.Designer.Layout.ViewModels.Documents;
using TextMateSharp.Grammars;

namespace Furesoft.LowCode.Designer.Layout.Views.Documents;

public partial class SourceDocumentView : UserControl
{
    private readonly RegistryOptions options = new(ThemeName.Dark);
    private TextMate.Installation _textmate;

    public SourceDocumentView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        Editor = this.FindControl<TextEditor>("Editor");

        Editor.TextArea.IndentationStrategy = new CSharpIndentationStrategy();
        _textmate = Editor.InstallTextMate(options);

        Editor.TextChanged += EditorOnTextChanged;
    }

    private void EditorOnTextChanged(object sender, EventArgs e)
    {
        var vm = (SourceDocumentViewModel)DataContext;
        vm.Source.Content = Editor.Text;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        var vm = (SourceDocumentViewModel)DataContext;

        if (string.IsNullOrEmpty(Editor.Document.Text))
        {
            Editor.Document.Text = vm.Source.Content;
        }

        _textmate.SetGrammar(options.GetScopeByExtension(".js"));
    }
}
