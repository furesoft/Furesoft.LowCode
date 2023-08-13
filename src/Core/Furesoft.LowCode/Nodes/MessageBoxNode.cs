using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.NodeViews;
using MsBox.Avalonia;

namespace Furesoft.LowCode.Nodes;

[NodeCategory]
[NodeView(typeof(IconNodeView),
    "M16,2H7.979C6.88,2,6,2.88,6,3.98V12c0,1.1,0.9,2,2,2h8c1.1,0,2-0.9,2-2V4C18,2.9,17.1,2,16,2z M16,12H8V4h8V12z M4,10H2v6  c0,1.1,0.9,2,2,2h6v-2H4V10z")]
[Description("Shows a message in a new window")]
public class MessageBoxNode : InputOutputNode
{
    public MessageBoxNode() : base("MessageBox")
    {
    }


    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The message to display")]
    [Required]
    public Evaluatable<string> Message { get; set; }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The message to display")]
    public string Title { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(Title, Evaluate(Message));

        await box.ShowWindowAsync();

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
