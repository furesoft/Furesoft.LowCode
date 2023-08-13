using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Filesystem;

[Description("Copies Filesystem Items")]
[NodeCategory("IO/FileSystem")]
public class CopyItemNode : InputOutputNode
{
    public CopyItemNode() : base("CopyItem")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Description("The Path of the items to be copied")]
    public Evaluatable<string> SourcePath { get; set; }

    [Description("The Path to the to the destination of the items")]
    [DataMember(EmitDefaultValue = false)]
    public Evaluatable<string> DestinationPath { get; set; }

    [Description("Should a folder be copied recursively. No effect on file copies.")]
    [DataMember(EmitDefaultValue = false)]
    public bool IsRecursive { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var folderDestinationPath = Path.GetDirectoryName(DestinationPath);
        if (!Directory.Exists(folderDestinationPath))
        {
            Directory.CreateDirectory(folderDestinationPath);
        }

        if (File.Exists(SourcePath))
        {
            var target = Path.Combine(Path.GetFileName(SourcePath), DestinationPath);
            File.Copy(SourcePath, target);
        }
        else if (Directory.Exists(SourcePath))
        {
            if (IsRecursive)
            {
                CopyFilesRecursively(SourcePath, DestinationPath);
            }
            else
            {
                var files = Directory.GetFiles(SourcePath);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var destination = Path.Combine(folderDestinationPath, fileName);
                    File.Copy(file, destination, true);
                }
            }
        }

        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }

    public static void CopyFilesRecursively(string sourcePath, string destinationPath)
    {
        if (Directory.Exists(sourcePath))
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            var entries = Directory.GetFileSystemEntries(sourcePath);

            foreach (var entry in entries)
            {
                var sourceEntryPath = Path.GetFullPath(entry);
                var destinationEntryPath = Path.Combine(destinationPath, Path.GetFileName(entry));
                if (Directory.Exists(sourceEntryPath))
                {
                    CopyFilesRecursively(sourceEntryPath, destinationEntryPath);
                }
                else
                {
                    File.Copy(sourceEntryPath, destinationEntryPath, true);
                }
            }
        }
    }
}
