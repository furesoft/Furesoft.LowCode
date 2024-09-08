using System.ComponentModel.DataAnnotations;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Filesystem;

[Description("Copies Filesystem Items")]
[NodeCategory("IO/FileSystem")]
[NodeIcon(
    "m 619.7 200 l 429.3 0 a 50 50 90 0 1 50 50 l 0 700 a 50 50 90 0 1 -50 50 l -900 0 a 50 50 90 0 1 -50 -50 l 0 -800 a 50 50 90 0 1 50 -50 l 370.7 0 l 100 100 z m -420.7 0 l 0 700 l 800 0 l 0 -600 l -420.7 0 l -100 -100 l -279.3 0 z m 400 350 l 0 -150 l 200 200 l -200 200 l 0 -150 l -200 0 l 0 -100 l 200 0 z")]
public class CopyItemNode() : InputOutputNode("CopyItem")
{
    [DataMember(EmitDefaultValue = false), Description("The Path of the items to be copied"), Required]
    public Evaluatable<string> SourcePath { get; set; }

    [Description("The Path to the to the destination of the items")]
    [DataMember(EmitDefaultValue = false)]
    [Required]
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

        return ContinueWith(OutputPin, cancellationToken);
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
