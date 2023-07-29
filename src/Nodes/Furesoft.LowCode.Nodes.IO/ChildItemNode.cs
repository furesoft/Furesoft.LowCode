using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Furesoft.LowCode.Designer.Core;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.IO.Enumeration;

namespace Furesoft.LowCode.Nodes.IO;
[Description("Get Items in a Folder")]
[NodeCategory("IO/Files")]
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
    
    [Description("Where to store the folder content")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string OutVariable { get; set; }

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
        DirectoryInfo dirInfo = new(folderPath);

        if (string.IsNullOrEmpty(SearchPattern))
        {
            SearchPattern = "*";
        }

        var fileInfos = ItemType switch
        {
            ItemType.File => dirInfo.GetFiles(SearchPattern, searchOption),
            ItemType.Folder => dirInfo.GetDirectories(SearchPattern, searchOption),
            ItemType.All => dirInfo.GetFileSystemInfos(SearchPattern, searchOption)
        };
        SetOutVariable(OutVariable, fileInfos);

        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
public enum ItemType
{
    File,
    Folder,
    All
}
