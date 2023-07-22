using System.Runtime.Serialization;
using Avalonia.Controls;
using Avalonia.Styling;
using NodeEditor.Model;
using NodeEditorDemo.Core.Components.Views;
using NodeEditorDemo.Core.NodeBuilding;

namespace NodeEditorDemo.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Value")]
[NodeView(typeof(MessageBoxView))]
public class MessageBoxNode : VisualNode
{
    private string _message;

    public MessageBoxNode() : base("MessageBox")
    {
        _message = "Hello Nodes :D";
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    [Pin("Flow Input", PinAlignment.Top)] 
    public IInputPin FlowInput { get; } = null;

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutput { get; } = null;

    public override void Evaluate()
    {
        new Window()
        {
            Content = new TextBlock {Text = _message},
            RequestedThemeVariant = ThemeVariant.Light,
            Width = 150,
            Height = 150
        }.Show();

        EvaluatePin(FlowOutput);
    }
}
