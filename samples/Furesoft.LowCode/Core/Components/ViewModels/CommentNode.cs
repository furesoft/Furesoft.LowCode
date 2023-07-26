using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Furesoft.LowCode.Core.Components.Views;
using Furesoft.LowCode.Core.NodeBuilding;

namespace Furesoft.LowCode.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(CommentView))]
[Description("A comment")]
public class CommentNode : VisualNode
{
    private string _comment = "This is a comment";

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Comment
    {
        get => _comment;
        set => SetProperty(ref _comment, value);
    }

    public CommentNode() : base("Comment")
    {
    }


    public override Task Execute()
    {
        return Task.CompletedTask;
    }
}
