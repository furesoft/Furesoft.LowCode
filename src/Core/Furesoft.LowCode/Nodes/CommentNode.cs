﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes;

[NodeView(typeof(CommentView))]
[Description("A comment")]
public class CommentNode : EmptyNode
{
    private string _comment = "This is a comment";

    public CommentNode() : base("Comment")
    {
    }

    [Browsable(false)] public new bool ShowDescription { get; set; }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Comment
    {
        get => _comment;
        set => SetProperty(ref _comment, value);
    }
}
