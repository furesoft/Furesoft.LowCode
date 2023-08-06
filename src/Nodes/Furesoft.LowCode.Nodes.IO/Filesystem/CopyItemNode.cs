namespace Furesoft.LowCode.Nodes.IO.Filesystem;

[Description("Copies Filesystem Items")]
[NodeCategory("IO/FileSystem")]
public class CopyItemNode : InputOutputNode
{
    [DataMember(EmitDefaultValue = false)]
    [Description("The Path of the items to be copied")]
    public string SourcePath { get; set; }
    
    [Description("The Path to the to the destination of the items")]
    [DataMember(EmitDefaultValue = false)]
    public string DestinationPath { get; set; }
    public CopyItemNode() : base("CopyItem")
    {
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var sourcePath = Evaluate<string>(SourcePath);
        var destinationPath = Evaluate<string>(DestinationPath);
        var folderDestinationPath = Path.GetDirectoryName(destinationPath);
        
        if (!Directory.Exists(folderDestinationPath))
        {
            Directory.CreateDirectory(folderDestinationPath);
        }
        if (File.Exists(sourcePath))
        {
            var target = Path.Combine(Path.GetFileName(sourcePath), destinationPath);
            File.Copy(sourcePath, target);
        }
        else if (Directory.Exists(sourcePath))
        {
            var files = Directory.GetFiles(sourcePath);
            
            foreach (string file in files)
            {
                var fileName = Path.GetFileName(file);
                var destination = Path.Combine(folderDestinationPath, fileName);

                File.Copy(file, destination, true);
            }
        }
        
        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
