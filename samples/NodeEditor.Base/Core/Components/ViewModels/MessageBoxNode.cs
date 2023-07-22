using System.Runtime.Serialization;
using System.Threading.Tasks;
using MsBox.Avalonia;
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
        _message = "42 + 3";
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    [Pin("Flow Input", PinAlignment.Top)] public IInputPin FlowInput { get; } = null;

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutput { get; } = null;

    public override async Task Execute()
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard("Info", Evaluator.Evaluate(_message).ToString());
        
        await box.ShowWindowAsync();

        await ExecutePin(FlowOutput);
    }
}
