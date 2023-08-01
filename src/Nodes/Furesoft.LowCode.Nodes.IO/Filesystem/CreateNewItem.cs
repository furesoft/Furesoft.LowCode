namespace Furesoft.LowCode.Nodes.IO.Filesystem;

[Description("Create Items in a Folder")]
[NodeCategory("IO/FileSystem")]
[NodeView(typeof(IconNodeView),
    "M67 216v-43h64v-64h42v64h64v43h-64v64h-42v-64h-64zM45 45c-24 0-42 19-42 43v213c0 24 18 43 42 43h342c24-0 42-19 42-43v-256c-0-24-18-43-42-43h-128l-43 43h-171z")]
public class CreateNewItem : InputOutputNode
{
    public CreateNewItem() : base("Create New Item")
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

    public override Task Execute(CancellationToken cancellationToken)
    {
        foreach (var item in TargetPaths)
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
