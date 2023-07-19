using System.Runtime.Serialization;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Value")]
public class TextNode : VisualNode
{
    private string? _text;

    public TextNode() : base("Text")
    {
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string? Text
    {
        get => _text;
        set => SetProperty(ref _text, value);
    }
}
