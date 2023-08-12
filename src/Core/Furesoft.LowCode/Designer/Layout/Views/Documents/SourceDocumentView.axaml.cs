using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;

namespace Furesoft.LowCode.Designer.Layout.Views.Documents;

public partial class SourceDocumentView : UserControl
{
    public SourceDocumentView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        Editor = this.FindControl<TextEditor>("Editor");

        Editor.TextArea.IndentationStrategy = new CSharpIndentationStrategy();
        var options = new RegistryOptions(ThemeName.Dark);
        var textmate = Editor.InstallTextMate(options);
        textmate.SetGrammar(options.GetScopeByExtension(".js"));
    }
}
