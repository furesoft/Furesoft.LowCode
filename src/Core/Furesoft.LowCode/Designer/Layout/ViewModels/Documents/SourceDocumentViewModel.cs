using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.ProjectSystem;

namespace Furesoft.LowCode.Designer.Layout.ViewModels.Documents;

public partial class SourceDocumentViewModel : Document
{
    public SourceDocumentViewModel(SourceFile source)
    {
        Source = source;
        Id = source.Name;
        Title = source.Name;
    }

    public SourceFile Source { get; set; }
}
