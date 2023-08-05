using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core.Components.Views;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[NodeView(typeof(CommentView))]
[Description("A comment")]
public class CommentNode : EmptyNode
{
    private string _comment = "This is a comment";

    public CommentNode() : base("Comment")
    {
    }
    
    public string Comment
    {
        get => _comment;
        set => SetProperty(ref _comment, value);
    }


    public override Task Execute(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
