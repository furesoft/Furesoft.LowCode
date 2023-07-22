using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Furesoft.LowCode.Core.Components.Views;
using Furesoft.LowCode.Core.NodeBuilding;
using MsBox.Avalonia;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Value")]
[NodeView(typeof(MessageBoxView))]
public class MessageBoxNode : VisualNode
{
    private string _message;

    public MessageBoxNode() : base("MessageBox")
    {
        _message = string.Empty;
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    [Browsable(false)]
    [Pin("Flow Input", PinAlignment.Top)] 
    public IInputPin FlowInput { get; } = null;

    [Browsable(false)]
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutput { get; } = null;

    public override async Task Execute()
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard("Info", Evaluate<string>(_message));
        
        await box.ShowWindowAsync();

        await ContinueWith(FlowOutput);
    }
}
