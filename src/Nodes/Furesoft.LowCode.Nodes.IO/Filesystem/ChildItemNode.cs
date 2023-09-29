using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Filesystem;

[Description("Get Items in a Folder")]
[NodeCategory("IO/FileSystem")]
[NodeIcon(
    "M896 209v-160c0-26.5-21.5-48-48-48h-160c-26.5 0-48 21.5-48 48v160c0 26.5 21.5 48 48 48h48v96h-256v-96h48c26.5 0 48-21.5 48-48v-160c0-26.5-21.5-48-48-48h-160c-26.5 0-48 21.5-48 48v160c0 26.5 21.5 48 48 48h48v96h-256v-96h48c26.5 0 48-21.5 48-48v-160c0-26.5-21.5-48-48-48h-160c-26.5 0-48 21.5-48 48v158c0 26.5 21.5 48 48 48h48v96c0 35 29 64 64 64h256v96h-48c-26.5 0-48 21.5-48 48v160c0 26.5 21.5 48 48 48h160c26.5 0 48-21.5 48-48v-160c0-26.5-21.5-48-48-48h-48v-96h256c35 0 64-29 64-64v-96h48c26.5 0 48-21.5 48-48z")]
internal class ChildItemNode : InputOutputNode, IOutVariableProvider, IPipeable
{
    public ChildItemNode() : base("Get Directory Items")
    {
    }

    [Description("The Path to the Folder to get the Items")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public Evaluatable<string> FolderPath { get; set; }

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

    [Description("Filters all Entries and excludes all Entries with that flag")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public FileAttributes ExcludedFlags { get; set; }

    [Description("The Variable to store the Output in.")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string OutVariable { get; set; }

    public object PipeVariable { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var result = ScriptInitalizer.ChildItem(IsRecurse, SearchPattern, FolderPath, FollowSymlink, ExcludedFlags, ItemType);

        if (result.IsSuccess)
        {
            PipeVariable = result.Value;

            if (!string.IsNullOrEmpty(OutVariable))
            {
                SetOutVariable(OutVariable, PipeVariable);
            }
        }
        else
        {
            throw (Exception)result.Value;
        }

        return ContinueWith(OutputPin, cancellationToken);
    }

    public override void Compile(CodeWriter builder)
    {
        CompileWriteCall(builder, "FS.childItem", IsRecurse, SearchPattern, FolderPath, FollowSymlink, ExcludedFlags, ItemType);
    }
}
