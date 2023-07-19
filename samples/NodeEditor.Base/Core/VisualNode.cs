using System.Runtime.Serialization;
using NodeEditorDemo.ViewModels;

namespace NodeEditorDemo.Core;

[DataContract(IsReference = true)]
public class VisualNode : ViewModelBase
{
    private string? _label;

    public VisualNode(string label)
    {
        Label = label;
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string? Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }
}
