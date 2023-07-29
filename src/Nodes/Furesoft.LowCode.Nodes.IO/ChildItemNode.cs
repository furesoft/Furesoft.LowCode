namespace Furesoft.LowCode.Nodes.IO;

[Description("Get Items in a Folder")]
[NodeCategory("IO/Directories")]
internal class ChildItemNode : VisualNode
{
    [Description("The Path to the Folder to get the Items")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string FolderPath { get; set; }

    [Description("Should the search be recursive")]
    [DataMember(IsRequired = false, EmitDefaultValue = true)]
    public bool IsRecurse { get; set; }

    [Description("How to filter the Entries")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string SearchPattern { get; set; }

    [Description("Which Items of a Folder should be selected")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public ItemType ItemType { get; set; } = ItemType.All;
    [Description("Should the symlink be followed")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public bool FollowSymlink { get; set; }

    [Description("Should only the Names be listed")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public bool OnlyName { get; set; }

    [Description("Where to store the folder content")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string OutVariable { get; set; }

    [Description("Filters all Entries and excludes all Entries with that flag")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public FileAttributes ExcludedFlags { get; set; } = new();

    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin InputPin { get; set; }

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin OutputPin { get; set; }

    public ChildItemNode() : base("Get Directory Items")
    {
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var folderPath = Evaluate<string>(FolderPath);
        var searchOption = IsRecurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

        if (string.IsNullOrEmpty(SearchPattern))
        {
            SearchPattern = "*";
        }

        DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
        if (FollowSymlink)
        {
            if (dirInfo.ResolveLinkTarget(true) is DirectoryInfo resolvedDir)
            {
                dirInfo = resolvedDir;
            }
            else
            {
                throw new Exception("Resolved Symlink doesn't link to a Directory");
            }
        }

        var fileInfos = ItemType switch
        {
            ItemType.File => dirInfo.GetFiles(SearchPattern, searchOption),
            ItemType.Folder => dirInfo.GetDirectories(SearchPattern, searchOption),
            ItemType.All => dirInfo.GetFileSystemInfos(SearchPattern, searchOption)
        };
        fileInfos = fileInfos.Where(x => !x.Attributes.HasFlag(ExcludedFlags)).ToArray();

        if (OnlyName)
        {
            SetOutVariable(OutVariable, fileInfos.Select(x => x.Name));
        }
        else
        {
            SetOutVariable(OutVariable, fileInfos);
        }

        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
public enum ItemType
{
    File,
    Folder,
    All
}
