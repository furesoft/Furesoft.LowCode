using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.ProjectSystem;

namespace Furesoft.LowCode.Designer.Layout.ViewModels.Documents;

public class SourceDocumentViewModel : Document
{
    public SourceDocumentViewModel(SourceFile source)
    {
        Source = source;
        Title = source.Name;
        Id = source.Id;
    }

    public SourceFile Source { get; set; }
}
