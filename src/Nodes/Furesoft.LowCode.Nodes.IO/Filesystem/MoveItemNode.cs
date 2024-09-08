using System.ComponentModel.DataAnnotations;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Filesystem;

[Description("Moves Filesystem Items")]
[NodeCategory("IO/FileSystem")]
[NodeIcon(
    "M 106.375 32.5 H 191.25 L 197.625 38.75 V 163.75 L 191.375 170 H 28.875 L 22.625 163.75 V 26.25 L 28.875 20 H 91.375 L 95.75 21.875 L 106.375 32.5 z M 184.875 157.5 V 138.625 L 185 88.625 V 69.875 H 106.25 L 95.5 80.625 L 91.125 82.5 H 35 V 157.5 H 184.875 z M 103.625 57.5 H 184.875 L 185 45.125 H 103.75 L 99.25 43.25 L 88.625 32.625 H 35 V 70.125 H 88.5 L 99.25 59.375 L 103.625 57.5 z M 132.25 108.375 L 115.25 91.25 L 124.5 82.625 L 152.25 110.375 V 119.25 L 123.625 146.875 L 114.875 137.875 L 132.375 121 H 115.25 A 25 25 90 0 0 98.375 128.625 A 22.5 22.5 90 0 0 91.75 145 H 79.25 A 34.5 34.5 90 0 1 89.375 120 A 37.5 37.5 90 0 1 114.375 108.375 H 132.25 z")]
public class MoveItemNode() : InputOutputNode("MoveItem")
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
        var sourcePath = SourcePath;
        var destinationPath = DestinationPath;
        TryCreateDirectory(destinationPath);
        // Check if the source is a directory
        var fileName = Path.GetFileName(sourcePath);
        var filePath = sourcePath + fileName;
        var destinationFilePath = destinationPath + fileName;
        if (File.Exists(filePath))
        {
            File.Copy(filePath, destinationFilePath, true);
            File.Delete(filePath);
        }
        else
        {
            CopyAndDeleteDirectory(sourcePath, destinationFilePath);
        }

        return ContinueWith(OutputPin, cancellationToken);
    }

    void TryCreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    void CopyAndDeleteDirectory(string sourcePath, string destinationPath)
    {
        var files = Directory.GetFiles(sourcePath);
        TryCreateDirectory(destinationPath);
        // Copy each file into destination directory
        foreach (var file in files)
        {
            var shortFileName = Path.GetFileName(file);
            var destFile = Path.Combine(destinationPath, shortFileName);
            File.Copy(file, destFile, true);
            File.Delete(file);
        }

        if (IsRecursive)
        {
            var directories = Directory.GetDirectories(sourcePath);
            foreach (var directory in directories)
            {
                var dirName = Path.GetFileName(directory);
                CopyAndDeleteDirectory(directory, destinationPath + dirName);
            }

            Directory.Delete(sourcePath);
        }
        else
        {
            if (!Directory.GetDirectories(sourcePath).Any())
            {
                Directory.Delete(sourcePath);
            }
        }
    }
}
