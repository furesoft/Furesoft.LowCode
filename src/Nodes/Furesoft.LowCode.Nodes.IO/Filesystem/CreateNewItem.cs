﻿using System.ComponentModel.DataAnnotations;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Filesystem;

[Description("Create Items in a Folder")]
[NodeCategory("IO/FileSystem")]
[NodeIcon(
    "M67 216v-43h64v-64h42v64h64v43h-64v64h-42v-64h-64zM45 45c-24 0-42 19-42 43v213c0 24 18 43 42 43h342c24-0 42-19 42-43v-256c-0-24-18-43-42-43h-128l-43 43h-171z")]
public class CreateNewItem() : InputOutputNode("Create New Item")
{
    [Description("Where should the items be created")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public BindingList<Evaluatable<string>> TargetPaths { get; set; } = new();

    [Description("What should the name of the item be")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Required]
    public string ItemName { get; set; }

    [Description("Which item should be created")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public ItemType ItemType { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        foreach (var path in TargetPaths)
        {
            switch (ItemType)
            {
                case ItemType.Directory:
                    Directory.CreateDirectory(Path.Combine(path, ItemName));
                    break;
                case ItemType.File:
                    File.Create(Path.Combine(path, ItemName));
                    break;
                default:
                    throw CreateError<ArgumentException>("Invalid item type");
            }
        }

        return ContinueWith(OutputPin, cancellationToken);
    }
}
