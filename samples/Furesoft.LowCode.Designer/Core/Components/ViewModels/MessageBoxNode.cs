using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Furesoft.LowCode.Designer.Core.Components.Views;
using Furesoft.LowCode.Editor.Model;
using MsBox.Avalonia;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory()]
[NodeView(typeof(IconNodeView), "M16,2H7.979C6.88,2,6,2.88,6,3.98V12c0,1.1,0.9,2,2,2h8c1.1,0,2-0.9,2-2V4C18,2.9,17.1,2,16,2z M16,12H8V4h8V12z M4,10H2v6  c0,1.1,0.9,2,2,2h6v-2H4V10z")]
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
    
    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin FlowInput { get; } = null;
    
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowOutput { get; } = null;

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(_title, Evaluate<string>(_message));

        await box.ShowWindowAsync();

        await ContinueWith(FlowOutput, cancellationToken: cancellationToken);
    }
}
