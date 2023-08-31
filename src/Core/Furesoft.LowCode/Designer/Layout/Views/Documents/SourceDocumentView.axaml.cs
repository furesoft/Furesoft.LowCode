using Avalonia.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;
using AvaloniaEdit.TextMate;
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

        _textmate.SetGrammar(options.GetScopeByExtension(".js"));
    }


    private void EditorOnTextChanged(object sender, EventArgs e)
    {
        var vm = (SourceDocumentViewModel)DataContext;
        vm.Source.Content = Editor.Text;
    }

    private async Task UpdateEditorTextAsync()
    {
        await Task.Delay(50); // Warte bis TextMate bereit ist.

        var vm = (SourceDocumentViewModel)DataContext;
        Editor.Document.Text = vm.Source.Content;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateEditorTextAsync();
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        Editor.TextChanged -= EditorOnTextChanged;
    }
}
