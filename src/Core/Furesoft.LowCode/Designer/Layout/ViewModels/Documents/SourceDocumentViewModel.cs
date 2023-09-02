using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.ProjectSystem.Items;

namespace Furesoft.LowCode.Designer.Layout.ViewModels.Documents;

public class SourceDocumentViewModel : Document
{
    public SourceDocumentViewModel(SourceFileItem source)
    {
        Source = source;
        Title = source.Name;
        Id = source.Id;
    }

    public SourceFileItem Source { get; set; }
}
