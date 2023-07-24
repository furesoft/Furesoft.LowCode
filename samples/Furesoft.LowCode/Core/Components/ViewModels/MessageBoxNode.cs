using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Furesoft.LowCode.Core.Components.Views;
using Furesoft.LowCode.Core.NodeBuilding;
using MsBox.Avalonia;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Base")]
[NodeView(typeof(MessageBoxView))]
[Description("Shows a message in a new window")]
public class MessageBoxNode : VisualNode
{
    private string _message;
    private string _title;

    public MessageBoxNode() : base("MessageBox")
    {
        _message = string.Empty;
        _title = "Info";
    }


    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The message to display")]
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The message to display")]
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
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
            .GetMessageBoxStandard(_title, Evaluate<string>(_message));

        await box.ShowWindowAsync();

        await ContinueWith(FlowOutput);
    }
}
