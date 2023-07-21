using System.Runtime.Serialization;
using NodeEditor.Model;
using NodeEditorDemo.Core.Components.Views;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Value")]
[NodeView(typeof(MessageBoxView))]
public class MessageBoxNode : VisualNode
{
    private string? _message;

    public MessageBoxNode() : base("MessageBox")
    {
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin FlowInput { get; set; }
    
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutput { get; set; }
}
