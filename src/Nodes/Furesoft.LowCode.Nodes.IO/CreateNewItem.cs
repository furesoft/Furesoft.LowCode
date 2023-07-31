using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace Furesoft.LowCode.Nodes.IO;
[Description("Create Items in a Folder")]
[NodeCategory("IO/FileSystem")]
public class CreateNewItem : VisualNode
{
    public CreateNewItem() : base(nameof(CreateNewItem))
    {
    }
    [Description("Where should the items be created")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public BindingList<string> TargetPaths { get; set; } = new();
    [Description("What should the name of the item be")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string ItemName { get; set; }
    [Description("Which item should be created")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public ItemTypes ItemType { get; set; }
    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin InputPin { get; set; }

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin OutputPin { get; set; }
    public override Task Execute(CancellationToken cancellationToken)
    {
        foreach(var item in TargetPaths)
        {
            var path = Evaluate<string>(item);
            switch (ItemType)
            {
                case ItemTypes.Directory:
                    Directory.CreateDirectory(Path.Combine(path, ItemName));
                    break;
                case ItemTypes.File:
                    File.Create(Path.Combine(path, ItemName));
                    break;
                default:
                    throw new ArgumentException("Invalid item type");
            }
        }

        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
public enum ItemTypes
{
    Directory,
    File
}
